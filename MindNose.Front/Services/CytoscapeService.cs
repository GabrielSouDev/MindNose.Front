using MindNose.Front.Models.Cytoscape;
using MindNose.Front.Models.Enum;
using Radzen.Blazor.Markdown;
using System.Collections.Generic;

namespace MindNose.Front.Services;

public class CytoscapeService
{
    private CytoscapeDTO _cytoscape;


    public CytoscapeService()
    {
        _cytoscape = new();

    }

    public void AddElements(ElementsDTO elements)
    {
        _cytoscape.Elements.Nodes = _cytoscape.Elements.Nodes.Concat(elements.Nodes)
                                                             .DistinctBy(n => n.Data.Id)
                                                             .ToList();

        _cytoscape.Elements.Edges = _cytoscape.Elements.Edges.Concat(elements.Edges)
                                                             .DistinctBy(n => n.Data.Id)
                                                             .ToList();
    }

    public ElementsDTO GetElements() => _cytoscape.Elements;
}
