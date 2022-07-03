/**
A simple classs to enable representing string through a float array.
This is a workaround for ASL's limitation of only allowing objects to send float arrays to each other.
IT'S ALL BLACK MAGIC, DON'T ASK JUST USE
*/
public static class StringToFloatArray
{
    public static float[] SToF(string input)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        float[] floats = new float[(bytes.Length - 1) / 4 + 1];
        System.Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);

        return floats;
    }

    public static string FToS(float[] input)
    {
        byte[] bytes = new byte[input.Length * 4];
        System.Buffer.BlockCopy(input, 0, bytes, 0, bytes.Length);

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
