/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.0
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace RakNet {

using System;
using System.Runtime.InteropServices;

public class NetworkIDObject : IDisposable {
  private HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal NetworkIDObject(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(NetworkIDObject obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~NetworkIDObject() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          RakNetPINVOKE.delete_NetworkIDObject(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
    }
  }

  NetworkIDManager oldManager;
  public virtual void SetNetworkIDManager(NetworkIDManager manager) 
  {
      if (oldManager != null)
      {
          oldManager.pointerDictionary.Remove(GetIntPtr());
      }
      if (manager != null)
      {
          manager.pointerDictionary.Add(GetIntPtr(), this);
          oldManager = manager;
      }
      SetNetworkIDManagerOrig(manager);
  }

  public IntPtr GetIntPtr()
  {
      return swigCPtr.Handle;
  }

  public NetworkIDObject() : this(RakNetPINVOKE.new_NetworkIDObject(), true) {
    SwigDirectorConnect();
  }

  protected void SetNetworkIDManagerOrig(NetworkIDManager manager) {
    if (this.GetType() == typeof(NetworkIDObject)) RakNetPINVOKE.NetworkIDObject_SetNetworkIDManagerOrig(swigCPtr, NetworkIDManager.getCPtr(manager)); else RakNetPINVOKE.NetworkIDObject_SetNetworkIDManagerOrigSwigExplicitNetworkIDObject(swigCPtr, NetworkIDManager.getCPtr(manager));
  }

  public virtual NetworkIDManager GetNetworkIDManager() {
    IntPtr cPtr = ((this.GetType() == typeof(NetworkIDObject)) ? RakNetPINVOKE.NetworkIDObject_GetNetworkIDManager(swigCPtr) : RakNetPINVOKE.NetworkIDObject_GetNetworkIDManagerSwigExplicitNetworkIDObject(swigCPtr));
    NetworkIDManager ret = (cPtr == IntPtr.Zero) ? null : new NetworkIDManager(cPtr, false);
    return ret;
  }

  public virtual ulong GetNetworkID() {
    ulong ret = ((this.GetType() == typeof(NetworkIDObject)) ? RakNetPINVOKE.NetworkIDObject_GetNetworkID(swigCPtr) : RakNetPINVOKE.NetworkIDObject_GetNetworkIDSwigExplicitNetworkIDObject(swigCPtr));
    return ret;
  }

  public virtual void SetNetworkID(ulong id) {
    if (this.GetType() == typeof(NetworkIDObject)) RakNetPINVOKE.NetworkIDObject_SetNetworkID(swigCPtr, id); else RakNetPINVOKE.NetworkIDObject_SetNetworkIDSwigExplicitNetworkIDObject(swigCPtr, id);
  }

  private void SwigDirectorConnect() {
    if (SwigDerivedClassHasMethod("SetNetworkIDManagerOrig", swigMethodTypes0))
      swigDelegate0 = new SwigDelegateNetworkIDObject_0(SwigDirectorSetNetworkIDManagerOrig);
    if (SwigDerivedClassHasMethod("GetNetworkIDManager", swigMethodTypes1))
      swigDelegate1 = new SwigDelegateNetworkIDObject_1(SwigDirectorGetNetworkIDManager);
    if (SwigDerivedClassHasMethod("GetNetworkID", swigMethodTypes2))
      swigDelegate2 = new SwigDelegateNetworkIDObject_2(SwigDirectorGetNetworkID);
    if (SwigDerivedClassHasMethod("SetNetworkID", swigMethodTypes3))
      swigDelegate3 = new SwigDelegateNetworkIDObject_3(SwigDirectorSetNetworkID);
    RakNetPINVOKE.NetworkIDObject_director_connect(swigCPtr, swigDelegate0, swigDelegate1, swigDelegate2, swigDelegate3);
  }

  private bool SwigDerivedClassHasMethod(string methodName, Type[] methodTypes) {
    System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, methodTypes, null);
    bool hasDerivedMethod = methodInfo.DeclaringType.IsSubclassOf(typeof(NetworkIDObject));
    return hasDerivedMethod;
  }

  private void SwigDirectorSetNetworkIDManagerOrig(IntPtr manager) {
    SetNetworkIDManagerOrig((manager == IntPtr.Zero) ? null : new NetworkIDManager(manager, false));
  }

  private IntPtr SwigDirectorGetNetworkIDManager() {
    return NetworkIDManager.getCPtr(GetNetworkIDManager()).Handle;
  }

  private ulong SwigDirectorGetNetworkID() {
    return GetNetworkID();
  }

  private void SwigDirectorSetNetworkID(ulong id) {
    SetNetworkID(id);
  }

  public delegate void SwigDelegateNetworkIDObject_0(IntPtr manager);
  public delegate IntPtr SwigDelegateNetworkIDObject_1();
  public delegate ulong SwigDelegateNetworkIDObject_2();
  public delegate void SwigDelegateNetworkIDObject_3(ulong id);

  private SwigDelegateNetworkIDObject_0 swigDelegate0;
  private SwigDelegateNetworkIDObject_1 swigDelegate1;
  private SwigDelegateNetworkIDObject_2 swigDelegate2;
  private SwigDelegateNetworkIDObject_3 swigDelegate3;

  private static Type[] swigMethodTypes0 = new Type[] { typeof(NetworkIDManager) };
  private static Type[] swigMethodTypes1 = new Type[] {  };
  private static Type[] swigMethodTypes2 = new Type[] {  };
  private static Type[] swigMethodTypes3 = new Type[] { typeof(ulong) };
}

}
