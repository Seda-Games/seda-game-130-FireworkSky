// 泛型委托定义,如果参数超出两个，可以继续往下定义

/////////////////////////// 不带返回值的委托 ///////////////////////////
public delegate void CallBack();  // 无参回调
public delegate void CallBack<T>(T arg); // 一个参数回调
public delegate void CallBack<T, A>(T arg1, A arg2); // 两个参数回调
public delegate void CallBack<T, A, B>(T arg1, A arg2, B arg3); // 三个参数回调

/////////////////////////// 带返回值的委托 ///////////////////////////
public delegate TResult ResultCallBack<out TResult>();  // 无参回调
public delegate TResult ResultCallBack<T, out TResult>(T arg); // 一个参数回调
public delegate TResult ResultCallBack<T, A, out TResult>(T arg1, A arg2); // 两个参数回调

