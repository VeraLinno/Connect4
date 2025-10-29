namespace DAL;

// <summary>
// Crud operations for game and configurations
//</summary>
//<typeparam name="TData">either Game or Configuration</typeparam>

public interface IRepository<TData>
{
    List<(string id, string description)> List();
    // crud
    
    string Save(TData data);
    TData Load(string id);
    void Delete(string id);
}