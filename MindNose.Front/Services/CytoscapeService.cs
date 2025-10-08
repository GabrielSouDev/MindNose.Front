using MindNose.Front.Models.Cytoscape;
using MindNose.Front.Models.Enum;
using Radzen.Blazor.Markdown;
using System.Collections.Generic;

namespace MindNose.Front.Services;

public class CytoscapeService
{
    private CytoscapeDTO _cytoscape;
    private CytoscapeLayout _layout;

    public CytoscapeService()
    {
        _cytoscape = new();
        _layout = CytoscapeLayout.cose;
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

    public CytoscapeLayout SetLayout(CytoscapeLayout newLayout) => _layout = newLayout;
    public CytoscapeLayout GetLayout() => _layout;
}
