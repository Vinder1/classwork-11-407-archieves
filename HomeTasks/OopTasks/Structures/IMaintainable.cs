namespace Structures;

public interface IMaintainable
{
    bool MaintenanceRequired();
    void Serve();
}