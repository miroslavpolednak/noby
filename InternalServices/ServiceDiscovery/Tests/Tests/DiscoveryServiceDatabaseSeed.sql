CREATE TABLE ServiceDiscovery (
	Id INTEGER PRIMARY KEY,
	EnvironmentName varchar(50) NULL,
	ServiceName varchar(50) NULL,
	ServiceUrl varchar(250) NULL,
	ServiceType tinyint NULL
);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('Integration', 'Storage', 'http://uat.storage',1);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('Integration', 'Notification', 'http://uat.notification',1);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('prod', 'Storage', 'http://prod.storage',1);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('prod', 'Notification', 'http://prod.notification',1);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('Integration', 'CIS:GlobalCache:Redis', '127.0.0.1:6379',1);
INSERT INTO [ServiceDiscovery] ([EnvironmentName], [ServiceName], [ServiceUrl], ServiceType) VALUES ('prod', 'CIS:GlobalCache:Redis', '127.0.0.1:6379',1);
