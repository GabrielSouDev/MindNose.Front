﻿namespace MindNose.Front.Models.Cytoscape;
public class ElementsDTO
{
    public List<NodeDTO> Nodes { get; set; } = new List<NodeDTO>();
    public List<EdgeDTO> Edges { get; set; } = new List<EdgeDTO>();
}
