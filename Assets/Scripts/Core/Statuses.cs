using System.ComponentModel;

namespace Statuses {

    public enum Status {
        [Description("Success")]
        SUCCESS = 0,
        [Description("Enemy stat redefinition error")]
        ENEMY_STAT_REDEFINITION,
    }
}

public static class StatusExtensions {

    // enum-string association method provided by stackoverflow user
    // Glennular at https://stackoverflow.com/a/630900
    public static string StatusString(this Statuses.Status statusInstance) {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])statusInstance
            .GetType()
            .GetField(statusInstance.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);

        if(attributes.Length == 0) { return "UNDEFINED STATUS"; }

        return attributes[0].Description;
    }

}
