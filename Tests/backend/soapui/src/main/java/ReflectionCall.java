import com.google.protobuf.ByteString;
import com.google.protobuf.DescriptorProtos;
import com.google.protobuf.Descriptors;
import com.google.protobuf.DynamicMessage;
import com.google.protobuf.InvalidProtocolBufferException;
import com.google.protobuf.TypeRegistry;
import com.google.protobuf.util.JsonFormat;
import io.grpc.*;
import io.grpc.netty.shaded.io.grpc.netty.GrpcSslContexts;
import io.grpc.netty.shaded.io.grpc.netty.NettyChannelBuilder;
import io.grpc.netty.shaded.io.netty.handler.ssl.util.InsecureTrustManagerFactory;
import io.grpc.protobuf.ProtoUtils;
import io.grpc.reflection.v1alpha.ServerReflectionGrpc;
import io.grpc.reflection.v1alpha.ServerReflectionRequest;
import io.grpc.reflection.v1alpha.ServerReflectionResponse;
import io.grpc.stub.ClientCalls;
import io.grpc.stub.StreamObserver;

import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.concurrent.Executor;
import java.util.logging.Logger;
import java.util.stream.Collectors;
import java.util.HashMap;

import org.json.JSONObject;

import com.google.common.base.Preconditions;

public class ReflectionCall {
    private static final Logger log = Logger.getLogger(ReflectionCall.class.getName());

    private Channel channel;
    //private CallCredentials cred;
    private String service;
    private int verboseLevel;
    HashMap<String, String> otherHeaders;
    private String basicAuth;

    private volatile String res;

    public ReflectionCall(String host, int port, String service, String basicAuth, int verboseLevel, HashMap<String, String> otherHeaders)
    {
	    this.service=service;
        this.verboseLevel=verboseLevel;
        this.basicAuth=basicAuth;
        if(otherHeaders==null)
            this.otherHeaders=new HashMap<String, String>();
        else
            this.otherHeaders=otherHeaders;

        try
        {
            
            Preconditions.checkArgument(1 >= 0.0, "negative value: %s", 1);
            channel = NettyChannelBuilder
                    .forAddress(host, port)
                    .sslContext(GrpcSslContexts.forClient().trustManager(InsecureTrustManagerFactory.INSTANCE).build())
                    .build();
        }
        catch(Exception e)
        {
            return;
        }
    }

    //TODO: Toto je debug - odstranit copy/paste code
    public ReflectionCall(String host, int port, String service, String basicAuth, int verboseLevel)
    {
	    this.service=service;
        this.verboseLevel=verboseLevel;
        this.basicAuth=basicAuth;
        this.otherHeaders=new HashMap<String, String>();

        try
        {
            channel = NettyChannelBuilder
                    .forAddress(host, port)
                    .sslContext(GrpcSslContexts.forClient().trustManager(InsecureTrustManagerFactory.INSTANCE).build())
                    .build();
        }
        catch(Exception e)
        {
            return;
        }
    }


    public JSONObject call( String methodSymbol, JSONObject requestContent, HashMap<String, String> callHeaders)
    {
        res=null;
        methodSymbol=service+"."+methodSymbol;
        String finalMethodSymbol = methodSymbol; //Kinda Java magic  

        CallCredentials headers=new CallCredentials()
        {
            @Override
            public void applyRequestMetadata(RequestInfo requestInfo, Executor executor, MetadataApplier metadataApplier)
            {
                Metadata meta=new io.grpc.Metadata();
                io.grpc.Metadata.Key<String> auth = io.grpc.Metadata.Key.of("authorization", Metadata.ASCII_STRING_MARSHALLER);
                meta.put(auth,basicAuth);

                for(String key : otherHeaders.keySet())
                {
                    io.grpc.Metadata.Key<String> metaKey = io.grpc.Metadata.Key.of(key, Metadata.ASCII_STRING_MARSHALLER);
                    meta.put(metaKey,otherHeaders.get(key));
                }

                for(String key : callHeaders.keySet())
                {
                    io.grpc.Metadata.Key<String> metaKey = io.grpc.Metadata.Key.of(key, Metadata.ASCII_STRING_MARSHALLER);
                    meta.put(metaKey,callHeaders.get(key));
                }                

                metadataApplier.apply(meta);
            }
            

            @Override
            public void thisUsesUnstableApi()
            {

            }
        };

        //if(verboseLevel > 0)
          //  log.info((verboseLevel>=2?"**************************************** ":"")+"Call to: "+methodSymbol+(verboseLevel>=2?" ****************************************":"")+
            //        (verboseLevel>=4?("\n"+requestContent.toString(2)+"\n************************************************************************************************************************"):""));

        // Building BlockingStub with Channel
        ServerReflectionGrpc.ServerReflectionStub reflectionStub = ServerReflectionGrpc.newStub(channel);//.withCallCredentials(cred); //Tady je pridavat cred zbytecne
        // response watcher
        StreamObserver<ServerReflectionResponse> streamObserver = new StreamObserver<ServerReflectionResponse>()
        {
            @Override
            public void onNext(ServerReflectionResponse response)
            {
                try {
                    // Only need to pay attention to the response of the file description type
                    if (response.getMessageResponseCase() == ServerReflectionResponse.MessageResponseCase.FILE_DESCRIPTOR_RESPONSE)
                    {
                        try
                        {
                            List<ByteString> fileDescriptorProtoList = response.getFileDescriptorResponse().getFileDescriptorProtoList();
                            handleResponse(fileDescriptorProtoList, channel, finalMethodSymbol, requestContent.toString(2),headers);
                        }
                        catch (Exception e)
                        {
                            res="{ \"ReflectionCallErrorDetail\":\""+e+"\",\"ReflectionCallError\":\""+e.getMessage()+"\"}";
                        }
                    }
                    else
                    {
                        res="{ \"ReflectionCallErrorDetail\":\"Unknown response type: "+response.getMessageResponseCase()+"\",\"ReflectionCallError\":\"Details: "+response.getErrorResponse()+"\"}";
                    }
                }
                catch (Exception e)
                {
                    res="{ \"ReflectionCallErrorDetail\":\""+e+"\",\"ReflectionCallError\":\""+e.getMessage()+"\"}";
                }
            }

            @Override
            public void onError(Throwable t)
            {
                //log.info("Running public void onError(Throwable t)");
            }

            @Override
            public void onCompleted()
            {
                //log.info("Complete");
            }
        };

        StreamObserver<ServerReflectionRequest> requestStreamObserver = reflectionStub.serverReflectionInfo(streamObserver);

        // Build and send get method file description request
        ServerReflectionRequest getFileContainingSymbolRequest = ServerReflectionRequest.newBuilder()
                .setFileContainingSymbol(methodSymbol)
                .build();
        requestStreamObserver.onNext(getFileContainingSymbolRequest);

        while(res==null);
        //log.info(res);
        //if(verboseLevel > 0)
          //  log.info((verboseLevel>=2?"**************************************** ":"")+"Return from "+methodSymbol+(verboseLevel>=2?" ****************************************":"")+
            //        (verboseLevel>=3?("\n"+res+"\n************************************************************************************************************************"):""));

        return new JSONObject(res);

    }

    public JSONObject call( String methodSymbol, JSONObject requestContent)
    {
        return call(methodSymbol, requestContent, (new HashMap<String, String>()));
    };

    /**
     * handle the response
     */
    private  void handleResponse(List<ByteString> fileDescriptorProtoList,
                                       Channel channel,
                                       String methodFullName,
                                       String requestContent,
                                       CallCredentials headers)
    {

        try {
            // Resolving method and service names
            String fullServiceName = extraPrefix(methodFullName);
            String methodName = extraSuffix(methodFullName);
            String packageName = extraPrefix(fullServiceName);
            String serviceName = extraSuffix(fullServiceName);

            // Parse FileDescriptor based on response
            Descriptors.FileDescriptor fileDescriptor = getFileDescriptor(fileDescriptorProtoList, packageName, serviceName);

            // Find service descriptions
            Descriptors.ServiceDescriptor serviceDescriptor = fileDescriptor.getFile().findServiceByName(serviceName);
            // Find method description
            Descriptors.MethodDescriptor methodDescriptor = serviceDescriptor.findMethodByName(methodName);

            // make a request
            executeCall(channel, fileDescriptor, methodDescriptor, requestContent, headers);
        }
        catch (Exception e)
        {
            //log.info(e.getMessage() + e);
            res="{ \"ReflectionCallErrorDetail\":\""+e+"\",\"ReflectionCallError\":\""+e.getMessage()+"\"}";
        }
    }

    /**
     * Parse and find the file description corresponding to the method
     */
    private  Descriptors.FileDescriptor getFileDescriptor(List<ByteString> fileDescriptorProtoList,
                                                                String packageName,
                                                                String serviceName) throws Exception {

        Map<String, DescriptorProtos.FileDescriptorProto> fileDescriptorProtoMap =
                fileDescriptorProtoList.stream()
                        .map(bs -> {
                            try {
                                return DescriptorProtos.FileDescriptorProto.parseFrom(bs);
                            } catch (InvalidProtocolBufferException e) {
                                e.printStackTrace();
                            }
                            return null;
                        })
                        .filter(Objects::nonNull)
                        .collect(Collectors.toMap(DescriptorProtos.FileDescriptorProto::getName, f -> f));


        if (fileDescriptorProtoMap.isEmpty()) {
            //log.info("service does not exist");
            throw new IllegalArgumentException("The file description for the method does not exist");
        }

        // Find the Proto description corresponding to the service
        DescriptorProtos.FileDescriptorProto fileDescriptorProto = findServiceFileDescriptorProto(packageName, serviceName, fileDescriptorProtoMap);

        // Get the dependencies of this Proto
        Descriptors.FileDescriptor[] dependencies = getDependencies(fileDescriptorProto, fileDescriptorProtoMap);

        // FileDescriptor that generates Proto
        return Descriptors.FileDescriptor.buildFrom(fileDescriptorProto, dependencies);
    }


    /**
     * Find the corresponding file description based on the package name and service name
     */
    private  DescriptorProtos.FileDescriptorProto findServiceFileDescriptorProto(String packageName,
                                                                                       String serviceName,
                                                                                       Map<String, DescriptorProtos.FileDescriptorProto> fileDescriptorProtoMap) {
        for (DescriptorProtos.FileDescriptorProto proto : fileDescriptorProtoMap.values()) {
            if (proto.getPackage().equals(packageName)) {
                boolean exist = proto.getServiceList()
                        .stream()
                        .anyMatch(s -> serviceName.equals(s.getName()));
                if (exist) {
                    return proto;
                }
            }
        }

        throw new IllegalArgumentException("service does not exist");
    }

    /**
     * get prefix
     */
    private  String extraPrefix(String content) {
        int index = content.lastIndexOf(".");
        return content.substring(0, index);
    }

    /**
     * get suffix
     */
    private  String extraSuffix(String content) {
        int index = content.lastIndexOf(".");
        return content.substring(index + 1);
    }

    /**
     * get dependency type
     */
    private  Descriptors.FileDescriptor[] getDependencies(DescriptorProtos.FileDescriptorProto proto,
                                                                Map<String, DescriptorProtos.FileDescriptorProto> finalDescriptorProtoMap) {
        return proto.getDependencyList()
                .stream()
                .map(finalDescriptorProtoMap::get)
                .map(f -> toFileDescriptor(f, getDependencies(f, finalDescriptorProtoMap)))
                .toArray(Descriptors.FileDescriptor[]::new);
    }

    /**
     * Convert FileDescriptorProto to FileDescriptor
     */
    private  Descriptors.FileDescriptor toFileDescriptor(DescriptorProtos.FileDescriptorProto fileDescriptorProto,
                                                               Descriptors.FileDescriptor[] dependencies)
    {
        try
        {
            return Descriptors.FileDescriptor.buildFrom(fileDescriptorProto, dependencies);
        }
        catch(Exception e)
        {
            //log.info("return Descriptors.FileDescriptor.buildFrom(fileDescriptorProto, dependencies); failed");
            return null;
        }
    }


    /**
     * execute method call
     */
    private void executeCall(Channel channel,
                                    Descriptors.FileDescriptor fileDescriptor,
                                    Descriptors.MethodDescriptor originMethodDescriptor,
                                    String requestContent,
                                    CallCredentials headers) throws Exception
    {

        // Regenerate MethodDescriptor
        MethodDescriptor<DynamicMessage, DynamicMessage> methodDescriptor = generateMethodDescriptor(originMethodDescriptor);

        CallOptions callOptions = CallOptions.DEFAULT;

        TypeRegistry registry = TypeRegistry.newBuilder()
                .add(fileDescriptor.getMessageTypes())
                .build();

        // Convert the request content from a JSON string to the corresponding type
        JsonFormat.Parser parser = JsonFormat.parser().usingTypeRegistry(registry);
        DynamicMessage.Builder messageBuilder = DynamicMessage.newBuilder(originMethodDescriptor.getInputType());
        parser.merge(requestContent+"  ", messageBuilder);
        DynamicMessage requestMessage = messageBuilder.build();

        // Call, the calling method can be inferred by originMethodDescriptor.isClientStreaming() and originMethodDescriptor.isServerStreaming()
        DynamicMessage response = ClientCalls.blockingUnaryCall(channel, methodDescriptor, callOptions.withCallCredentials(headers), requestMessage);

        // Parse the response as a JSON string
        JsonFormat.Printer printer = JsonFormat.printer()
                .usingTypeRegistry(registry)
                .includingDefaultValueFields();
        String responseContent = printer.print(response);

        res=responseContent;
    }

    /**
     * Regenerate method description
     */
    private  MethodDescriptor<DynamicMessage, DynamicMessage> generateMethodDescriptor(Descriptors.MethodDescriptor originMethodDescriptor) {
        // Generate method full name
        String fullMethodName = MethodDescriptor.generateFullMethodName(originMethodDescriptor.getService().getFullName(), originMethodDescriptor.getName());
        // Request and Response Type
        MethodDescriptor.Marshaller<DynamicMessage> inputTypeMarshaller = ProtoUtils.marshaller(DynamicMessage.newBuilder(originMethodDescriptor.getInputType())
                .buildPartial());
        MethodDescriptor.Marshaller<DynamicMessage> outputTypeMarshaller = ProtoUtils.marshaller(DynamicMessage.newBuilder(originMethodDescriptor.getOutputType())
                .buildPartial());

        // Generate method description, fullMethodName of originMethodDescriptor is incorrect
        return MethodDescriptor.<DynamicMessage, DynamicMessage>newBuilder()
                .setFullMethodName(fullMethodName)
                .setRequestMarshaller(inputTypeMarshaller)
                .setResponseMarshaller(outputTypeMarshaller)
                // Use UNKNOWN to automatically modify
                .setType(MethodDescriptor.MethodType.UNKNOWN)
                .build();
    }
}