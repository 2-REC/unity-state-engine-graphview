using System.Collections.Generic;
using System.Xml.Serialization;


// TODO: implement interface 'IXmlData'?
public class XmlState {

    [XmlAttribute]
    public string id;

    [XmlAttribute]
    public string scene;

    [XmlAttribute]
    public string next;

    [XmlAttribute]
    public bool restartable;

    [XmlAttribute]
    public bool isLevel;

    [XmlArray, XmlArrayItem("child")]
    public List<string> children;
}
