using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Validate.LogicLayer
{
    public class ObjPaint
    {
    }

    public class GenericObj
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string xPosition { get; set; }
        public string yPosition { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class GraphicNodes : GenericObj
    {
        public string Type { get; set; }
        public string EventType { get; set; }
        public string error { get; set; }
    }

    public class TransitionNodes
    {
        public string Id { get; set; }
        public string FromX { get; set; }
        public string FromY { get; set; }
        public string ToX { get; set; }
        public string ToY { get; set; }
    }

    public class MessagesFlows : TransitionNodes
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public class CompleteGraphic
    {
        public List<GenericObj> WorkProcess { get; set; }
        public List<GraphicNodes> ActivitiesToPaint { get; set; }
        public List<TransitionNodes> TransitionsToPaint { get; set; }
        public List<MessagesFlows> Messages { get; set; }

    }
}