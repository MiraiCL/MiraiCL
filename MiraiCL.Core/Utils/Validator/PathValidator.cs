namespace MiraiCL.Core.Utils.Validator;

public class PathValidator(string input):IValidator{
    public string[] Check(){
        #if WINDOWS

        #endif
        return [];
    }
}