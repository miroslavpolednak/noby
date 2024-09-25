namespace NOBY.ApiContracts;

public partial class AdminUpdateAdminFeBannerRequest
    : IRequest
{
    [JsonIgnore]
    public int FeBannerId { get; private set; }

    public AdminUpdateAdminFeBannerRequest InfuseId(int feBannerId)
    {
        this.FeBannerId = feBannerId;
        return this;
    }
}
