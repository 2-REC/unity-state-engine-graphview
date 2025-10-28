using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;


// TODO: should make 2 classes Global and Game?
// TODO: implement interface 'IXmlData'?
public class XmlState {

    [XmlAttribute]
    public string id;

    [DefaultValue("")]
    [XmlAttribute]
    public string scene;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool isLevel;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool restartable;

    [DefaultValue(false)]
    [XmlAttribute]
    public bool leavable;

    [XmlAttribute]
    public string next;

    [XmlArray, XmlArrayItem("child")]
    public List<XmlChild> children;

}
