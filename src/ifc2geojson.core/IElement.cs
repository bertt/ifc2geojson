

using System.Collections.Generic;

namespace ifc2geojson.core
{
    public interface IElement
    {
        string Name { get; set; }

        string Description { get; set; }

        string GlobalId { get; set; }

        Dictionary<string, object> Properties { get; set; }

    }
}
