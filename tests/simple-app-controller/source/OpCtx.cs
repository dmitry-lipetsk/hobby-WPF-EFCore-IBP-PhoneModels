////////////////////////////////////////////////////////////////////////////////

namespace simple_app_controller{
////////////////////////////////////////////////////////////////////////////////
//class OpCtx

sealed class OpCtx
{
 public OpCtx(Logger logger)
 {
  m_Logger=logger;
 }

 //-----------------------------------------------------------------------
 public Logger Logger
 {
  get
  {
   return m_Logger;
  }//get
 }//Logger

 //-----------------------------------------------------------------------
 private readonly Logger m_Logger;
};//class OpCtx

////////////////////////////////////////////////////////////////////////////////
}//namespace simple_app_controller
