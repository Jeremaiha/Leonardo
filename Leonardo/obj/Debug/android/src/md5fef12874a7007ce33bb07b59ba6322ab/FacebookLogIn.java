package md5fef12874a7007ce33bb07b59ba6322ab;


public class FacebookLogIn
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Leonardo.FacebookLogIn, Leonardo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FacebookLogIn.class, __md_methods);
	}


	public FacebookLogIn () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FacebookLogIn.class)
			mono.android.TypeManager.Activate ("Leonardo.FacebookLogIn, Leonardo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
