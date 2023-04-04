namespace NOBY.Api.Endpoints.DocumentOnSA.GetDocumentsSignList;

public enum SignatureStateE
{
    Unknown = 0,
    Ready = 1,
    InTheProcess= 2,
    WaitingForScan = 3,
    Signed = 4
}
