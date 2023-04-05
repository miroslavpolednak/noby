using System.Text;
using System.Xml.Linq;

namespace MPSS.Security.Noby;

internal sealed class ConfigurationCache
{
    private static volatile ConfigurationCache instance = null!;
    private static object syncRoot = new Object();

    public Configuration Configuration { get; private set; }

    /// <summary>
    /// ctr
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ConfigurationCache() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static ConfigurationCache Instance(int? environmentId, string? securityConfigurationFile)
    {
        if (instance == null)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            lock (syncRoot)
            {
                if (instance == null)
                {
                    var cls = new ConfigurationCache();

                    Configuration c = new Configuration()
                    {
                        EnvironmentId = environmentId.GetValueOrDefault()
                    };
                    // read config file
                    XDocument xdoc = XDocument.Load(securityConfigurationFile!);
                    if (xdoc == null)
                        throw new Exception("Configuration file can't be read");

                    // neni zname id prostredi
                    if (!environmentId.HasValue)
                    {
                        c.EnvironmentId = Convert.ToInt32(xdoc.Root
                            .Element("defaults")
                            .Elements()
                            .FirstOrDefault(t => (string)t.Attribute("key") == "EnvironmentType")
                            ?.Attribute("value").Value);
                    }
                    if (c.EnvironmentId == 0)
                        throw new Exception("Environment ID unknown");

                    // server
                    c.Server = Convert.ToInt32(xdoc.Root
                            .Element("defaults")
                            .Elements()
                            .FirstOrDefault(t => (string)t.Attribute("key") == "Server")
                            ?.Attribute("value").Value);

                    // nacist konfiguraci
                    var envElement = xdoc.Root.Elements("environment");
                    if (envElement == null)
                        throw new Exception($"Configuration section 'environment' not found");
                    var section = envElement.Where(t => (int)t.Attribute("type")! == c.EnvironmentId).FirstOrDefault();
                    if (section == null)
                        throw new Exception($"Configuration section {c.EnvironmentId} not found");

                    // cookie info
                    foreach (var el in section.Elements("add"))
                    {
                        switch (el.Attribute("key").Value)
                        {
                            case "DbConnectionString":
                                c.DbConnectionString = el.Attribute("value").Value;
                                break;
                            case "LogConnectionString":
                                c.LogConnectionString = el.Attribute("value").Value;
                                break;
                            case "CookieValidatorSpecialIPs":
                                c.CookieValidatorSpecialIPs = el.Attribute("value").Value.Split(',');
                                break;

                            case "CookieName":
                                c.CookieName = el.Attribute("value").Value;
                                break;
                            case "CookieDomainName":
                                c.CookieDomainName = el.Attribute("value").Value;
                                break;
                            case "CookiePath":
                                c.CookiePath = el.Attribute("value").Value;
                                break;
                            case "PersistentCookie":
                                c.PersistentCookie = el.Attribute("value").Value.ToLower() == "true";
                                break;
                            case "CookieExpiration":
                                c.CookieExpiration = Convert.ToInt32(el.Attribute("value").Value);
                                break;
                            case "CookieOnlyHttps":
                                c.CookieOnlyHttps = el.Attribute("value").Value.ToLower() == "true";
                                break;
                            case "MaxCookieAge":
                                c.MaxCookieAge = Convert.ToInt32(el.Attribute("value").Value);
                                break;

                            case "RijndaelPwdIterations":
                                c.RijndaelPwdIterations = Convert.ToInt32(el.Attribute("value").Value);
                                break;
                            case "RijndaelStrength":
                                c.RijndaelStrength = Convert.ToInt32(el.Attribute("value").Value);
                                break;
                            case "DbLogMinSeverity":
                                c.DbLogMinSeverity = Convert.ToInt32(el.Attribute("value").Value);
                                break;
                            case "RijndealPassword":
                                c.RijndealPassword = el.Attribute("value").Value;
                                break;
                            case "RijndaelVector":
                                var iv = Encoding.ASCII.GetBytes(el.Attribute("value").Value);
                                if (iv.Length > 16)
                                {
                                    byte[] iv2 = new byte[16];
                                    Array.Copy(iv, iv2, iv2.Length);
                                    c.RijndaelVector = iv2;
                                }
                                else
                                    c.RijndaelVector = iv;
                                break;
                            case "RijndaelSalt":
                                c.RijndaelSalt = ASCIIEncoding.UTF8.GetBytes(el.Attribute("value").Value);
                                break;
                            case "PMPLogoutUrl":
                                c.PMPLogoutUrl = el.Attribute("value").Value;
                                break;
                            case "PMPLoginUrl":
                                c.PMPLoginUrl = el.Attribute("value").Value;
                                break;
                            case "PMPMainUrl":
                                c.PMPMainUrl = el.Attribute("value").Value;
                                break;
                            case "PMPAppProxyUrl":
                                c.PMPAppProxyUrl = el.Attribute("value").Value;
                                break;
                            case "PMPExtAppProxyUrl":
                                c.PMPExtAppProxyUrl = el.Attribute("value").Value;
                                break;
                        }
                    }

                    cls.Configuration = c;

                    // registrace logovaci DB provideru
                    Log.LoggerFactory.AddLogger(new Log.DbLogger());

                    instance = cls;
                }
            }
        }

        return instance;
    }
}
