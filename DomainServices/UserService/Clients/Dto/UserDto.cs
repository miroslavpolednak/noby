﻿using DomainServices.UserService.Contracts;
using ProtoBuf;

namespace DomainServices.UserService.Clients.Dto;

[ProtoContract]
public sealed class UserDto
{
    [ProtoMember(1)]
    public int UserId {  get; set; }

    [ProtoMember(2)]
#pragma warning disable CA2227 // Collection properties should be read only
    public List<SharedTypes.GrpcTypes.UserIdentity> UserIdentifiers { get; set; } = null!;
#pragma warning restore CA2227 // Collection properties should be read only

    [ProtoMember(3)]
    public UserInfoObject UserInfo { get; set; } = null!;

    [ProtoMember(4)]
#pragma warning disable CA2227 // Collection properties should be read only
    public List<int> UserPermissions { get; set; } = [];
#pragma warning restore CA2227 // Collection properties should be read only

    [ProtoContract]
#pragma warning disable CA1034 // Nested types should not be visible
    public sealed class UserInfoObjectDto
#pragma warning restore CA1034 // Nested types should not be visible
    {
        [ProtoMember(20)]
        public string FirstName { get; set; } = string.Empty;

        [ProtoMember(21)]
        public string LastName { get; set; } = string.Empty;

        [ProtoMember(22)]
        public string DisplayName { get; set; } = string.Empty;

        [ProtoMember(23)]
        public string? Cpm { get; set; }
        
        [ProtoMember(24)]
        public string? Icp { get; set; }

        [ProtoMember(25)]
        public string? Cin { get; set; }

        [ProtoMember(26)]
        public string? PhoneNumber { get; set; }

        [ProtoMember(27)]
        public string? Email { get; set; }

        [ProtoMember(28)]
        public bool IsUserVIP { get; set; }

        [ProtoMember(29)]
        public bool IsInternal { get; set; }

        [ProtoMember(30)]
        public int ChannelId { get; set; }

        [ProtoMember(31)]
        public string? PersonOrgUnitName { get; set; }

        [ProtoMember(32)]
        public string? DealerCompanyName { get; set; }
    }
}
