using System.Text;

namespace MPSS.Security.Noby;

/// <summary>
/// <para>Kontajner pro ulozeni informaci o soucasnem uzivateli, ktery se serializuje do cookie.</para>
/// <para>Obsahuje vlastnosti pro overovani platnosti cookie.</para>
/// </summary>
[Serializable]
internal class SecurityCookie
{
    internal static char[] Separator = new char[] { '~' };

    /// <summary>
    /// Instance uzivatele.
    /// </summary>
    public IdentityBase Identity { get; set; }

    /// <summary>
    /// <para>(Overovanni platnosti) Posledni update cookie. Mel by byt stejny jako cookie.expiration.</para>
    /// <para>Cas se overuje jeste jednou aby nebylo mozne podvrhnout jiz proslou cookie s platnym cookie.expiration.</para>
    /// </summary>
    public DateTime LastUpdate { get; set; }

    /// <summary>
    /// (Overovanni platnosti) IP adresa vzdaleneho pocitace ze soucasneho pozadavku.
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// (Overovanni platnosti) User agent vzdaleneho pocitace ze soucasneho pozadavku.
    /// </summary>
    public string UserAgent { get; set; }

    // default constructor
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SecurityCookie() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0}{1}", LastUpdate.ToString(System.Globalization.CultureInfo.InvariantCulture), Separator[0]);
        sb.AppendFormat("{0}{1}", IP, Separator[0]);
        sb.AppendFormat("{0}{1}", UserAgent, Separator[0]);
        sb.Append(Identity.ToString());
        sb.Append(Separator[0] + Guid.NewGuid().ToString());

        return sb.ToString();
    }

    public byte[] ToByteArray()
    {
        return System.Text.UTF8Encoding.UTF8.GetBytes(this.ToString());
    }

    internal static SecurityCookie GetFromByteArray(byte[] bytes)
    {
        return GetFromString(System.Text.UTF8Encoding.UTF8.GetString(bytes));
    }

    internal static SecurityCookie GetFromString(string value)
    {
        SecurityCookie cookie = new SecurityCookie();
        string[] arr = value.Split(Separator, 4);
        cookie.IP = arr[1];
        cookie.UserAgent = arr[2];
        cookie.Identity = IdentityBase.GetFromString(arr[3]);
        try
        {
            cookie.LastUpdate = Convert.ToDateTime(arr[0], System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception err)
        {
            throw new Exception("SecurityCookie.GetFromString(): error converting LastUpdate to DateTime from '" + arr[0] + "'", err);
        }

        return cookie;
    }
}
