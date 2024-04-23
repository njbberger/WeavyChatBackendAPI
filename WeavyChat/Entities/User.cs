using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Identity;

namespace WeavyChat.Entities
{
    public class User
    {
        public enum Status : byte
        {
            [Description("PENDING")] Pending = 0,
            [Description("ACTIVE")] Active = 1,
            [Description("INACTIVE")] Inactive = 2,
            [Description("ARCHIVE")] Archive = 3
        }

        public string UserName { get; set; } = null!;
        public Status Statum { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Password { get; set; } = null!;
        //public DateTime? LastLoginAt { get; set; }
        //public short FailedLogins { get; set; }
        //public bool LoginLocked { get; set; }
        //public DateTime? LoginLockedAt { get; set; }
        //public KSUid OrgId { get; set; } = null!;
        public string OrgId { get; set; }
        public string OrgRoleName { get; set; }
        //public Portals Portal { get; set; }
        //public SystemRoles SystemRole { get; set; }
        //public KSUid? OrgRole { get; set; }
        //public string LangId { get; set; } = null!;
        //public KSUid? LocationId { get; set; }
        //public KSUid? UnitId { get; set; }
        //public string? MobilePhone { get; set; }
        //public string? SecondaryPhone { get; set; }
        //public KSUid? ModifiedBy { get; set; }
        //public DateTime? ModifiedAt { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string? EmailToken { get; set; }
        //public string VerificationCode { get; set; }
        //public DateTime VerificationCodeCreatedAt { get; set; }
        //public DateTime? EmailVerificationAt { get; set; }
    }

    public class UserSync
    {
        public class Input 
        {
            public int id { get; set; }
            public string uid { get; set; }
            public string display_name { get; set; }
            public string email { get; set; }
            public string phone_number { get; set; }
            public string name { get; set; } //first name
            public string family_name { get; set; } //lastname
            //public string? given_name { get; set; }
            //public string? middle_name { get; set; }
            //public string nickname { get; set; }
            public string? comment { get; set; }
            //public int directory_id { get; set; } //org id
            public string directory { get; set; } //org id
            //public WeavyDirectory directory { get; set; } = new WeavyDirectory(); //org id + org_name
            //
            //public int? picture_id { get; set; }
            //public string avatar_url { get; set; }
            //public object? metadata { get; set; }
            //public string[]? tags { get; set; }
            //public string? presence { get; set; }
            public string created_at { get; set; }
            public string modified_at { get; set; }
            public bool is_bot { get; set; }  //false
            public bool is_suspended { get; set; } //false
            public bool is_trashed { get; set; } //false
        }
        public class Output 
        {
            public int id { get; set; }
            public string uid { get; set; }
            public string display_name { get; set; }
            public string email { get; set; }
            public string phone_number { get; set; }
            public string name { get; set; } //first name
            public string family_name { get; set; } //lastname
            //public string? given_name { get; set; }
            //public string? middle_name { get; set; }
            //public string nickname { get; set; }
            public string? comment { get; set; }
            public int directory_id { get; set; } //org id
            //public string directory { get; set; } //org id
            public WeavyDirectory directory { get; set; } = new WeavyDirectory(); //org id + org_name
            
            //public int? picture_id { get; set; }
            //public string avatar_url { get; set; }
            //public object? metadata { get; set; }
            //public string[]? tags { get; set; }
            //public string? presence { get; set; }
            public string created_at { get; set; }
            public string modified_at { get; set; }
            public bool is_bot { get; set; }  //false
            public bool is_suspended { get; set; } //false
            public bool is_trashed { get; set; } //false

            //{
            //  "id": "integer",
            //  "uid": "string",
            //  "display_name": "string",
            //  "email": "string",
            //  "given_name": "string",
            //  "middle_name": "string",
            //  "name": "string",
            //  "family_name": "string",
            //  "nickname": "string",
            //  "phone_number": "string",
            //  "comment": "string",
            //  "directory": {
            //    "id": "integer",
            //    "name": "string"
            //  },
            //  "directory_id": "integer",
            //  "picture": {
            //    "id": "integer",
            //    "name": "string",
            //    "media_type": "string",
            //    "width": "integer",
            //    "height": "integer",
            //    "size": "integer",
            //    "thumbnail_url": "string",
            //    "raw": "string"
            //  },
            //  "picture_id": "integer",
            //  "avatar_url": "string",
            //  "metadata": "object",
            //  "tags": [
            //    "string"
            //  ],
            //  "presence": "string",
            //  "created_at": "string",
            //  "modified_at": "string",
            //  "is_bot": "boolean",
            //  "is_suspended": "boolean",
            //  "is_trashed": "boolean"
            //}
        }
    }

    public class WeavyDirectory
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class UserList
    {
        public class InputUserList
        {
            public bool bot { get; set; }
            public int? directory_id { get; set; } = null;//List users in the specified directory, null lists users from all directories
            public bool suspended { get; set; } = true; //Indicates whether to list suspended users or not
            public string? q { get; set; } //A query used to find matching items
            public string? tag { get; set; } //List items with the specified tag
            public bool trashed { get; set; } = false;//Indicates whether trashed items should be listed (default is false). Specify null to return both trashed and non-trashed items.
            public string? order_by { get; set; } //Specifies the sort order and direction for the listing, e.g. "name" or "name+desc"
            public int? top { get; set; } //Maximum number of items to return in the listing. Should be a value between 1 and 100. Default is 25           
            public int? skip { get; set; } //The number of items to skip. Used together with top to return a specific range of items (for pagination)            
            public bool count_only { get; set; } //true to only return the number of matching items; when this is specified the response will only contain the count property.
        }
        public class OutputUserList
        {
            public List<UsersData> data { get; set; } = new List<UsersData>();
            public int start { get; set; }
            public int end { get; set; }
            public int count { get; set; }

            //{
            //  "data": [
            //    {
            //      "id": "integer",
            //      "uid": "string",
            //      "display_name": "string",
            //      "email": "string",
            //      "given_name": "string",
            //      "middle_name": "string",
            //      "name": "string",
            //      "family_name": "string",
            //      "nickname": "string",
            //      "phone_number": "string",
            //      "comment": "string",
            //      "directory_id": "integer",
            //      "picture_id": "integer",
            //      "avatar_url": "string",
            //      "metadata": "object",
            //      "tags": [
            //        "string"
            //      ],
            //      "presence": "string",
            //      "created_at": "string",
            //      "modified_at": "string",
            //      "is_bot": "boolean",
            //      "is_suspended": "boolean",
            //      "is_trashed": "boolean"
            //    }
            //  ],
            //  "start": "integer",
            //  "end": "integer",
            //  "count": "integer"
            //}
        }
    }

    public class UsersData
    {
        public int id { get; set; }
        public string uid { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string? given_name { get; set; }
        public string? middle_name { get; set; }        
        public string name { get; set; } //first name
        public string family_name { get; set; } //lastname
        public string nickname { get; set; }
        public string phone_number { get; set; }
        public string? comment { get; set; }
        public int directory_id { get; set; } //org id
        public int? picture_id { get; set; }
        public string avatar_url { get; set; }
        public object? metadata { get; set; }
        //public string[]? tags { get; set; }
        public List<string>? tags { get; set; } = new List<string>();
        public string? presence { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public bool is_bot { get; set; }  //false
        public bool is_suspended { get; set; } //false
        public bool is_trashed { get; set; } //false
    }
}
