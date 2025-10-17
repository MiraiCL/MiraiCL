using System.Text.RegularExpressions;

namespace MiraiCL.Core.FileIO;

public static class FileUtils{
    public static async Task RemoveAsync(string path){
        await Task.Run(() =>{
            foreach(var file in EnumerateFile(path)??[]){
                try{
                    File.Delete(file.FullName);
                }catch(IOException){

                }catch(UnauthorizedAccessException){

                }
            }
        });
    }

    public static IEnumerable<FileInfo>? EnumerateFile(string path,bool ignoreEx = true){
        try{
            return new DirectoryInfo(path).EnumerateFiles();
        }catch(Exception){
            if (ignoreEx) return null;
            throw;
        }
    }
    ///<summary>
    /// Enumerate the specified directory and filter the results based on the function's return value.
    ///</summary>
    public static IEnumerable<FileInfo>? SearchFile(string targetPath,string fileName,Func<FileInfo,bool> searchRule){
        return EnumerateFile(targetPath)?.Where((f) => f.Name.Contains(fileName,StringComparison.OrdinalIgnoreCase))
            .Where(searchRule);
    }
    ///<summary>
    /// Enumerate and search the specified directory, and limit the search results to match part of the file name while excluding certain keywords.
    ///</summary>
    public static IEnumerable<FileInfo>? SearchFile(string targetPath,string fileName,string[] excludeIfPathContains) 
        =>  EnumerateFile(targetPath)?.Where(f => f.Name.Contains(fileName,StringComparison.OrdinalIgnoreCase))
            .Where(f => !excludeIfPathContains
                .Any(ignore => f.FullName.Contains(ignore,StringComparison.OrdinalIgnoreCase)));
    ///<summary>
    /// Enumerate and search the specified directory, restricting the search results to only those that match the specified regular expression.
    ///</summary>
    public static IEnumerable<FileInfo>? SearchFile(string targetPath,Regex searchRules) 
        => EnumerateFile(targetPath)?.Where(f => searchRules.IsMatch(f.DirectoryName??string.Empty)); 
}

