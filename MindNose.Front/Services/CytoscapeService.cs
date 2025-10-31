using MindNose.Front.Models.Cytoscape;
using MindNose.Front.Models.Enum;
using MindNose.Front.Models.IAChat;
using Radzen.Blazor.Markdown;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace MindNose.Front.Services;

public class CytoscapeService
{
    private CytoscapeDTO _cytoscape;
    private List<string> _selectedElements;
    private List<CategoryResponse> _categories;
    private CytoscapeLayout _layout;

    public CytoscapeService()
    {
        _cytoscape = new();
        _selectedElements = new();
        _categories = new();
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
    public void AddSelectedElement(string elementId) => _selectedElements.Add(elementId);
    public void RemoveSelectedElement(string elementId) => _selectedElements.Remove(elementId);
    public List<string> GetSelectedElements() => _selectedElements;
    public ElementsDTO GetElements() => _cytoscape.Elements;
    public void SetLayout(CytoscapeLayout newLayout) => _layout = newLayout;
    public CytoscapeLayout GetLayout() => _layout;
    public void SetCategories(List<CategoryResponse> categories) => _categories = categories;
    public List<CategoryResponse> GetCategories() => _categories;
    public List<ElementHeader> GetSelectedElementsHeader()
    {
        if (_selectedElements is not null)
        {
            var selectedElements = _cytoscape.Elements.Nodes.Where(e => _selectedElements.Contains(e.Data.Id)).ToList();
            var selectedHeader = selectedElements
                        .OrderBy(sh => sh.Data.Label == "Category")
                        .Select(e =>
                            new ElementHeader
                            {
                                Id = e.Data.Id,
                                Title = e.Data.Extra!["Title"].ToString()!,
                                Summary = e.Data.Extra["Summary"].ToString()!,
                                Type = e.Data.Label!
                            })
                        .DistinctBy(e => e.Id)
                        .ToList();

            return selectedHeader;
        }
        return new();
    }
    public List<ElementHeader> GetNotSelectedElementsHeader()
    {
        if (_cytoscape.Elements.Nodes.Count > _selectedElements.Count)
        {
            var notSelectedElements = _cytoscape.Elements.Nodes.Where(e => !_selectedElements.Contains(e.Data.Id)).ToList();
            var notSelectedHeader = notSelectedElements
                        .OrderBy(sh => sh.Data.Label == "Category")
                        .Select(e =>
                            new ElementHeader
                            {
                                Id = e.Data.Id,
                                Title = e.Data.Extra!["Title"].ToString()!,
                                Summary = e.Data.Extra["Summary"].ToString()!,
                                Type = e.Data.Label!
                            })
                        .DistinctBy(e => e.Id)
                        .ToList();

            return notSelectedHeader;
        }
        return new();
    }
}
