namespace Owlvey.Falcon.Core.Models.Migrate
{
    public interface ISheet
    {
        int getRows(); 
        T get<T>(int row, int column);
        
    }

}