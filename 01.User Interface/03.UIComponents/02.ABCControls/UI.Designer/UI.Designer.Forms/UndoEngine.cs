using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace ABCControls
{
    public class UndoEngineImpl : UndoEngine
    {
        private string _Name_="UndoEngineImpl";

        private Stack<UndoEngine.UndoUnit> undoStack=new Stack<UndoEngine.UndoUnit>();
        private Stack<UndoEngine.UndoUnit> redoStack=new Stack<UndoEngine.UndoUnit>();
        private bool _canUndo=false;
        private bool _canRedo=false;

        public event EventHandler CanUndoChanged;
        public event EventHandler CanRedoChanged;

        public bool CanUndo
        {
            get { return _canUndo; }
            set
            {
                _canUndo=value;
                CanUndoChanged( value , null );
            }
        }

        public bool CanRedo
        {
            get { return _canRedo; }
            set
            {
                _canRedo=value;
                CanRedoChanged( value , null );
            }
        }

        public UndoEngineImpl ( IServiceProvider provider ) : base( provider ) { }

        public void CleanEngine ( )
        {
            undoStack.Clear();
            redoStack.Clear();
            _canRedo=false;
            _canUndo=false;
        }

        public bool EnableUndo
        {
            get { return undoStack.Count>0; }
        }

        public bool EnableRedo
        {
            get { return redoStack.Count>0; }
        }

        public void Undo ( )
        {
            if ( undoStack.Count>0 )
            {
                try
                {
                    UndoEngine.UndoUnit unit=undoStack.Pop();
                    unit.Undo();
                    redoStack.Push( unit );
                    //Update the CanUndo and CanRedo state
                    CanUndo=undoStack.Count==0?false:true;
                    CanRedo=redoStack.Count==0?false:true;
                    //Log("::Undo - undo action performed: " + unit.Name);
                }
                catch ( Exception ex )
                {
                    //Log("::Undo() - Exception " + ex.Message + " (line:" + new StackFrame(true).GetFileLineNumber() + ")");
                }
            }
            else
            {
                //Log("::Undo - NO undo action to perform!");
            }
        }

        public void Redo ( )
        {
            if ( redoStack.Count>0 )
            {
                try
                {
                    UndoEngine.UndoUnit unit=redoStack.Pop();
                    unit.Undo();
                    undoStack.Push( unit );
                    //Update the CanUndo and CanRedo state
                    CanUndo=undoStack.Count==0?false:true;
                    CanRedo=redoStack.Count==0?false:true;
                    //Log("::Redo - redo action performed: " + unit.Name);
                }
                catch ( Exception ex )
                {
                    //Log("::Redo() - Exception " + ex.Message + " (line:" + new StackFrame(true).GetFileLineNumber() + ")");
                }
            }
            else
            {
                //Log("::Redo - NO redo action to perform!");
            }
        }


        protected override void AddUndoUnit ( UndoEngine.UndoUnit unit )
        {
            undoStack.Push( unit );
            CanUndo=true;
        }
        //List<UndoEngine.UndoUnit> undoUnitList=new List<UndoUnit>();

        //// points to the command that should be executed for Redo
        //int currentPos=0;

        //public UndoEngineImpl ( IServiceProvider provider )
        //    : base( provider )
        //{
        //}

        //public void DoUndo ( )
        //{
        //    if ( currentPos>0 )
        //    {
        //        UndoEngine.UndoUnit undoUnit=undoUnitList[currentPos-1];
        //        undoUnit.Undo();
        //        currentPos--;
        //    }
        //    UpdateUndoRedoMenuCommandsStatus();
        //}

        //public void DoRedo ( )
        //{
        //    if ( currentPos<undoUnitList.Count )
        //    {
        //        UndoEngine.UndoUnit undoUnit=undoUnitList[currentPos];
        //        undoUnit.Undo();
        //        currentPos++;
        //    }
        //    UpdateUndoRedoMenuCommandsStatus();
        //}

        //private void UpdateUndoRedoMenuCommandsStatus ( )
        //{
        //    // this components maybe cached.
        //    object svc=GetService( typeof( IMenuCommandService ) );
        //    MenuCommandService menuCommandService=svc as MenuCommandService;
        //    if ( menuCommandService==null )
        //    {
        //        Type svcType=svc.GetType();
        //        System.Reflection.PropertyInfo[] props=svcType.GetProperties();
        //        for ( int i=0; i<props.Length; i++ )
        //        {
        //            if ( props[i].Name=="MenuService" )
        //            {
        //                menuCommandService=props[0].GetValue( svc , new object[0] ) as MenuCommandService;
        //                break;
        //            }
        //        }
        //    }

        //    if ( menuCommandService==null )
        //        return;

        //    MenuCommand undoMenuCommand=menuCommandService.FindCommand( StandardCommands.Undo );
        //    MenuCommand redoMenuCommand=menuCommandService.FindCommand( StandardCommands.Redo );

        //    if ( undoMenuCommand!=null )
        //        undoMenuCommand.Enabled=currentPos>0;
        //    if ( redoMenuCommand!=null )
        //        redoMenuCommand.Enabled=currentPos<this.undoUnitList.Count;
        //}

        //protected override void AddUndoUnit ( UndoEngine.UndoUnit unit )
        //{
        //    undoUnitList.RemoveRange( currentPos , undoUnitList.Count-currentPos );
        //    undoUnitList.Add( unit );
        //    currentPos=undoUnitList.Count;

        //    UpdateUndoRedoMenuCommandsStatus();
        //}

        //protected override UndoEngine.UndoUnit CreateUndoUnit ( string name , bool primary )
        //{
        //    return base.CreateUndoUnit( name , primary );
        //}

        //protected override void DiscardUndoUnit ( UndoEngine.UndoUnit unit )
        //{
        //    undoUnitList.Remove( unit );
        //    base.DiscardUndoUnit( unit );

        //    UpdateUndoRedoMenuCommandsStatus();
        //}



        //public string[] GetAvailableUndoActions ( )
        //{
        //    return GetAvailableUndoActions( int.MaxValue );
        //}
        //public string[] GetAvailableUndoActions ( int maxNumber )
        //{
        //    if ( currentPos==0 )
        //        return null;

        //    int actionsCount=( currentPos<maxNumber )?currentPos:maxNumber;
        //    string[] undoActions=new string[actionsCount];
        //    for ( int i=0; i<actionsCount; i++ )
        //    {
        //        undoActions[i]=undoUnitList[currentPos-i-1].Name;
        //    }

        //    return undoActions;
        //}

        //public string[] GetAvailableRedoActions ( )
        //{
        //    return GetAvailableRedoActions( int.MaxValue );
        //}
        //public string[] GetAvailableRedoActions ( int maxNumber )
        //{
        //    if ( currentPos==undoUnitList.Count )
        //        return null;

        //    int actionsCount=undoUnitList.Count-currentPos;
        //    if ( actionsCount>maxNumber )
        //        actionsCount=maxNumber;
        //    string[] redoActions=new string[actionsCount];
        //    int counter=0;
        //    for ( int i=currentPos; i<undoUnitList.Count&&counter<actionsCount; i++ , counter++ )
        //    {
        //        redoActions[counter]=undoUnitList[i].Name;
        //    }

        //    return redoActions;
        //}
    }
}
