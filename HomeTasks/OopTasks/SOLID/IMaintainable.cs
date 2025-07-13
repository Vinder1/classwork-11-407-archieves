namespace SOLID;

public interface IMaintainable
{
    bool MaintenanceRequired();
    void Serve();
}