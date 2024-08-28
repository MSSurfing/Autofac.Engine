/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;
using System.Threading.Tasks;
using System.Threading;

namespace MSSurfing.TClient.Domain.Users
{

  /// <summary>
  /// User
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class TUser : TBase
  {
    private int _Id;
    private string _Name;
    private string _Mobilephone;

    /// <summary>
    /// Id
    /// </summary>
    public int Id
    {
      get
      {
        return _Id;
      }
      set
      {
        __isset.Id = true;
        this._Id = value;
      }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
      get
      {
        return _Name;
      }
      set
      {
        __isset.Name = true;
        this._Name = value;
      }
    }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string Mobilephone
    {
      get
      {
        return _Mobilephone;
      }
      set
      {
        __isset.Mobilephone = true;
        this._Mobilephone = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool Id;
      public bool Name;
      public bool Mobilephone;
    }

    public TUser() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 5:
              if (field.Type == TType.I32) {
                Id = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 15:
              if (field.Type == TType.String) {
                Name = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 20:
              if (field.Type == TType.String) {
                Mobilephone = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("TUser");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.Id) {
          field.Name = "Id";
          field.Type = TType.I32;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Id);
          oprot.WriteFieldEnd();
        }
        if (Name != null && __isset.Name) {
          field.Name = "Name";
          field.Type = TType.String;
          field.ID = 15;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Name);
          oprot.WriteFieldEnd();
        }
        if (Mobilephone != null && __isset.Mobilephone) {
          field.Name = "Mobilephone";
          field.Type = TType.String;
          field.ID = 20;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Mobilephone);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("TUser(");
      bool __first = true;
      if (__isset.Id) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Id: ");
        __sb.Append(Id);
      }
      if (Name != null && __isset.Name) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Name: ");
        __sb.Append(Name);
      }
      if (Mobilephone != null && __isset.Mobilephone) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Mobilephone: ");
        __sb.Append(Mobilephone);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

        public Task ReadAsync(TProtocol tProtocol, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(TProtocol tProtocol, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}