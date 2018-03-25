using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using ABCBusinessEntities;

namespace ABCProvider
{
    public interface IFileProvider
    {
        #region File

        bool IsFileAvaiable ( Guid ID );

        Guid UploadFile ( GEFilesInfo fileInfo );
        Guid UploadFile ( Guid ownerID , String strLocalPath , String strFileName , double Revision , String strPass );

        bool CachingFile ( Guid ID );
        bool IsFileCached ( Guid ID );
        void CleanCachedFiles ( );
        void AutoCacheOwnFiles ( Guid userID );

        bool OpenFile ( Guid ID );
        bool SaveAsFileToLocal ( Guid ID , String strLocalPath , bool isOpenWhenCompleted );
        bool DeleteFile ( Guid ID );
        bool UpdateFile ( Guid ID , String strLocalPath );

        #endregion

        bool AttachFileToVoucher ( Guid fileID , String strTableName , Guid ID );
        bool AttachFileToVoucher ( Guid fileID , String strTableName , Guid ID , String strDisplayName , String strDisplayShortName , String strPass );
        bool AttachFileToComment ( Guid fileID , Guid commentID );
        bool AttachFileToComment ( Guid fileID , Guid commentID , String strDisplayName , String strDisplayShortName , String strPass );


        bool AssignFileToDocument ( Guid fileID , Guid documentID );
    }

    public class FileProvider //: IFileProvider
    {
   
    }
}
