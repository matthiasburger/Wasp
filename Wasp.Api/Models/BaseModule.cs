namespace Wasp.Api.Models
{
    public class BaseModule
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PythonId { get; set; }
        
        public virtual void OnInit()
        {
            
        }
    }
}