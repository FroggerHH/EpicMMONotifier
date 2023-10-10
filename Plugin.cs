using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using DiscordMessenger;

#pragma warning disable CS8632
namespace EpicMMONotifier;

[BepInDependency("WackyMole.EpicMMOSystem")]
[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string
        ModName = "EpicMMONotifier",
        ModVersion = "1.1.0",
        ModGUID = $"com.{ModAuthor}.{ModName}",
        ModAuthor = "Frogger";

    private static ConfigEntry<string> titleMessegesListConfig;
    private static ConfigEntry<string> descriptionMessegesListConfig;
    private static ConfigEntry<string> usernamesListConfig;
    private static ConfigEntry<string> colorsListConfig;
    private static ConfigEntry<string> webhookConfig;
    public List<string> titleMessegesList = new() { "Level UP!", " New level!", "Greetings new level!" };
    public List<string> usernamesList = new() { "EpicMMONotifier", " EpicMMO", "Notifier" };

    public List<string> descriptionMessegesList = new()
    {
        "#PLAYERNICK has risen to level #NEWLEVEL!", "#PLAYERNICK has received level #NEWLEVEL!",
        "#PLAYERNICK is now level #NEWLEVEL!"
    };

    public List<string> colorsList = new() { "1752220", "3447003", "15844367", "15105570", "7506394" };
    public string webhook;

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion);
        OnConfigurationChanged += UpdateConfiguration;
        titleMessegesListConfig = config("General", "Title Messeges",
            "Level UP! , New level! , Greetings new level!", "");
        descriptionMessegesListConfig = config("General", "Description Messeges",
            "#PLAYERNICK has risen to level #NEWLEVEL! , #PLAYERNICK has received level #NEWLEVEL! , #PLAYERNICK is now level #NEWLEVEL!",
            "");
        usernamesListConfig = config("General", "Usernames", "EpicMMONotifier, EpicMMO, Notifier", "");
        colorsListConfig = config("General", "Colors", "1752220, 3447003, 15844367, 15105570, 7506394",
            "Attention! Use only the colors from this table! \nhttps://gist.github.com/thomasbnt/b6f455e2c7d743b796917fa3c205f812?permalink_comment_id=3656937#gistcomment-3656937");
        webhookConfig = config("General", "Webhook", "", "");
    }

    private void UpdateConfiguration()
    {
        titleMessegesList = new();
        string titleMessegesListString = titleMessegesListConfig.Value;
        string[] titleMesseges = titleMessegesListString.Split(',');
        foreach (string msg in titleMesseges) titleMessegesList.Add(msg);

        descriptionMessegesList = new();
        string descriptionMessegesListString = descriptionMessegesListConfig.Value;
        string[] descriptionMesseges = descriptionMessegesListString.Split(',');
        foreach (string msg in descriptionMesseges) descriptionMessegesList.Add(msg);

        usernamesList = new();
        string usernamesListString = usernamesListConfig.Value;
        string[] usernameMesseges = usernamesListString.Split(',');
        foreach (string msg in usernameMesseges) usernamesList.Add(msg);

        colorsList = new();
        string colorsListString = colorsListConfig.Value;
        string[] colorsMesseges = colorsListString.Split(',');
        foreach (string msg in colorsMesseges) colorsList.Add(msg);

        webhook = webhookConfig.Value;

        Debug("Configuration Received");
    }

    public void SendDiscordMessage(string playerNick = "none", int newLevel = 0)
    {
        var random = Random.Range(0, titleMessegesList.Count);
        string title;
        if (random <= titleMessegesList.Count) title = titleMessegesList[random];
        else title = titleMessegesList[random - 1];
        title = title.Replace("#PLAYERNICK", playerNick);
        title = title.Replace("#NEWLEVEL", newLevel.ToString());

        random = Random.Range(0, descriptionMessegesList.Count);
        string description;
        if (random <= descriptionMessegesList.Count) description = descriptionMessegesList[random];
        else description = descriptionMessegesList[random - 1];
        description = description.Replace("#PLAYERNICK", playerNick);
        description = description.Replace("#NEWLEVEL", newLevel.ToString());

        random = Random.Range(0, usernamesList.Count);
        string username;
        if (random <= usernamesList.Count) username = usernamesList[random];
        else username = usernamesList[random - 1];

        string color;
        random = Random.Range(0, colorsList.Count);
        if (random <= colorsList.Count) color = colorsList[random];
        else color = colorsList[random - 1];

        new DiscordMessage()
            .SetUsername(username)
            .SetAvatar(
                "https://gcdn.thunderstore.io/live/repository/icons/LambaSun-EpicMMOSystem-1.2.8.png.128x128_q95.png")
            .AddEmbed()
            .SetTitle(title)
            .SetDescription(description)
            .SetColor(int.Parse(color))
            .Build()
            .SendMessageAsync(webhook);
    }
}