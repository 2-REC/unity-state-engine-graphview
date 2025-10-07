using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;


// TODO: should make 2 classes Global and Game?
// TODO: implement interface 'IXmlData'?
public class XmlState {

    [XmlAttribute]
    public string id;

    [XmlAttribute]
    public string scene;

    [XmlAttribute]
    public string next;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool restartable;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool leavable;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool isLevel;

    [XmlArray, XmlArrayItem("child")]
    public List<string> children;
}
