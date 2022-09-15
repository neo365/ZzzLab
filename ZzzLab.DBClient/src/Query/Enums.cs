namespace ZzzLab.Data
{
    public enum Direction : int
    {
        Input = 0x01,
        Output = 0x02,
        ReturnValue = 0x04,
        InputOutput = Input | Output
    }
}