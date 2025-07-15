using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("graph")]
public class XmlGraph : IXmlData {

    // TODO: init here?
    [XmlArray, XmlArrayItem("state")]
    public List<XmlState> states = new();
}
