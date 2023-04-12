namespace MPSS.Security.Noby;

public interface IPortal
{
    string CreateCookieValue(
        string cpm, /* CPM uzivatele */
        string icp, /* ICP uzivatele */
        string name, /* jmeno a prijmeni uzivatele */
        int id, /* v33id uzivatele */
        int s53id, /* s53id uzivatele - 0 pokud se nejedna o uzivatele z t01stat_mesic */
        int v11id, /* v11id uzivatele - 0 pokud se nejedna o zamestnance */
        int v03id, /* v03id uzivatele - 0 pokud se nejedna o agenturu */
        int m17id,
        int brokerId);
}
