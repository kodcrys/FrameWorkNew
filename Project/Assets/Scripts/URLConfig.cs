
public class URLConfig
{
    public static string dbFunctionAddress = "https://us-central1-manhngfinal.cloudfunctions.net/api/dataFunction/";
    //public static string dbFunctionAddress = "http://localhost:5000/manhngfinal/us-central1/api/dataFunction/";
   
    public static string getAlldbAddress = "getAllDb";
    public static string getDataAddress = "getData";
    public static string setNewdbAddress = "setNewDataPlayer";
    public static string updateDataAddress = "updateData";
    public static string updateSingleDataAddress = "updateSingleData";
}

public class PathData
{
    public static string root_db = "PlayerData";
    public static string playerInfoS = "playerInfo";
    public static string playerInfo = "PlayerData/playerInfo";
    public static string playerInventory = "playerInventory";
    public static string coin = "playerInventory/coin";
}
