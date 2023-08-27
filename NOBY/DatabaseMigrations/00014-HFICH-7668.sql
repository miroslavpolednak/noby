INSERT INTO dbo.FlowSwitch (FlowSwitchId, [name], [Description], DefaultValue) VALUES (15,'IsRealEstateValuationAllowed','Je možné oceňovat nemovitost',0);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (5,1,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,1,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,1,3,1);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (5,2,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,2,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,2,3,1);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (5,3,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,3,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,3,3,1);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,4,2,0);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,5,2,0);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (5,6,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,6,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,6,3,1);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (2,7,2,1);

INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (3,12,3,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (5,12,3,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,12,2,1);
INSERT INTO FlowSwitch2Group (FlowSwitchGroupId, FlowSwitchId, GroupType, [Value]) VALUES (6,12,3,1);

DELETE FROM FlowSwitch2Group WHERE FlowSwitchGroupId=8 AND FlowSwitchId=1 and GroupType=2;
