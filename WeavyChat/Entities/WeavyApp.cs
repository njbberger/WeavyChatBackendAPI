using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel;

namespace WeavyChat.Entities
{
    public class WeavyApp
    {
        public enum Access : byte
        {
            [Description("NONE")] none = 0, //non-members cannot access the app
            [Description("READ")] read = 1, //non-members can see the app and it's content but can cannot contribute
            [Description("WRITE")] write = 2 //non-members have access to the app and are allowed to create content
        }

        public class InputGetApp
        {            
            public string uid { get; set; } = null!; //UserName
            public bool trashed { get; set; } = false;
        }

        public class OutputGetApp
        {
            public int id { get; set; }
            public string type { get; set; }
            public string uid { get; set; }
            public string access { get; set; }
            public int directory_id { get; set; } //org id
            public string display_name { get; set; }
            public string name { get; set; } 
            public string description { get; set; }
            public string? archive_url { get; set; }
            public string? avatar_url { get; set; }
            public object? metadata { get; set; }
            public List<string>? tags { get; set; } = new List<string>();
            public string created_at { get; set; }
            public int? created_by_id { get; set; }
            public By created_by { get; set; } = new By();
            public string modified_at { get; set; }
            public int? modified_by_id { get; set; }
            public By modified_by { get; set; } = new By();
            public Members members { get; set; } = new Members();
            public List<string>? permissions { get; set; } = new List<string>();
            public bool is_starred { get; set; }
            public bool is_subscribed { get; set; }
            public bool is_trashed { get; set; }

            //{
            //  "id": "integer",
            //  "type": "string",
            //  "uid": "string",
            //  "access": "string",
            //  "directory_id": "integer",
            //  "display_name": "string",
            //  "name": "string",
            //  "description": "string",
            //  "archive_url": "string",
            //  "avatar_url": "string",
            //  "metadata": "object",
            //  "tags": [
            //    "string"
            //  ],
            //  "created_at": "string",
            //  "created_by_id": "integer",
            //  "created_by": {
            //    "id": "integer",
            //    "uid": "string",
            //    "display_name": "string",
            //    "email": "string",
            //    "given_name": "string",
            //    "middle_name": "string",
            //    "name": "string",
            //    "family_name": "string",
            //    "nickname": "string",
            //    "phone_number": "string",
            //    "comment": "string",
            //    "directory_id": "integer",
            //    "picture_id": "integer",
            //    "avatar_url": "string",
            //    "metadata": "object",
            //    "tags": [
            //      "string"
            //    ],
            //    "presence": "string",
            //    "created_at": "string",
            //    "modified_at": "string",
            //    "is_bot": "boolean",
            //    "is_suspended": "boolean",
            //    "is_trashed": "boolean"
            //  },
            //  "modified_at": "string",
            //  "modified_by_id": "integer",
            //  "modified_by": {
            //    "id": "integer",
            //    "uid": "string",
            //    "display_name": "string",
            //    "email": "string",
            //    "given_name": "string",
            //    "middle_name": "string",
            //    "name": "string",
            //    "family_name": "string",
            //    "nickname": "string",
            //    "phone_number": "string",
            //    "comment": "string",
            //    "directory_id": "integer",
            //    "picture_id": "integer",
            //    "avatar_url": "string",
            //    "metadata": "object",
            //    "tags": [
            //      "string"
            //    ],
            //    "presence": "string",
            //    "created_at": "string",
            //    "modified_at": "string",
            //    "is_bot": "boolean",
            //    "is_suspended": "boolean",
            //    "is_trashed": "boolean"
            //  },
            //  "members": {
            //    "data": [
            //      "object"
            //    ],
            //    "start": "integer",
            //    "end": "integer",
            //    "count": "integer"
            //  },
            //  "permissions": [
            //    "string"
            //  ],
            //  "is_starred": "boolean",
            //  "is_subscribed": "boolean",
            //  "is_trashed": "boolean"
            //}
        }

        public class InputListApp 
        {
            //Query parameters
            public bool contextual { get; set; } //true to lists contextual apps, false to list non-contextual apps; when not specified both types are listed. Default value is true.            
            public List<string>? type { get; set; } = new List<string>();//Guids of app types to list. Can be used to return only conversations of a specified type. When not specied all types of apps are returned.
            //NON-CONTEXTUAL apps:
            //"type":"7e14f418-8f15-46f4-b182-f619b671e470" - can NOT add members (private chat)
            //"type":"edb400ac-839b-45a7-b2a8-6a01820d1c44" - can add members

            //CONTEXTUAL apps:
            //"type":"d65dd4bc-418e-403c-9f56-f9cf4da931ed" - can add members

            public string? q { get; set; } //A query used to find matching items
            public string? tag { get; set; } //List items with the specified tag
            public bool trashed { get; set; } = false;//Indicates whether trashed items should be listed (default is false). Specify null to return both trashed and non-trashed items.
            public string? order_by { get; set; } //Specifies the sort order and direction for the listing, e.g. "name" or "name+desc"
            public int? top { get; set; } //Maximum number of items to return in the listing. Should be a value between 1 and 100. Default is 25           
            public int? skip { get; set; } //The number of items to skip. Used together with top to return a specific range of items (for pagination)            
            public bool count_only { get; set; } //true to only return the number of matching items; when this is specified the response will only contain the count property.    
        }
        public class OutputListApp
        {
            public List<AppsData> data { get; set; } = new List<AppsData>();
            public int start { get; set; }
            public int end { get; set; }
            public int count { get; set; }

            //{
            //  "data": [
            //    {
            //      "id": "integer",
            //      "type": "string",
            //      "uid": "string",
            //      "access": "string",
            //      "directory_id": "integer",
            //      "display_name": "string",
            //      "name": "string",
            //      "description": "string",
            //      "archive_url": "string",
            //      "avatar_url": "string",
            //      "metadata": "object",
            //      "tags": [
            //        "string"
            //      ],
            //      "created_at": "string",
            //      "created_by_id": "integer",
            //      "modified_at": "string",
            //      "modified_by_id": "integer",
            //      "permissions": [
            //        "string"
            //      ],
            //      "is_starred": "boolean",
            //      "is_subscribed": "boolean",
            //      "is_trashed": "boolean"
            //    }
            //  ],
            //  "start": "integer",
            //  "end": "integer",
            //  "count": "integer"
            //}

        }

        public class AppsData
        {
            public int id { get; set; }
            public string type { get; set; }
            public string uid { get; set; }            
            public string access { get; set; }
            public int directory_id { get; set; } //org id
            public string? display_name { get; set; }
            public string name { get; set; } //first name
            public string description { get; set; }
            public string archive_url { get; set; }
            public string avatar_url { get; set; }          
            public object? metadata { get; set; }
            //public string[]? tags { get; set; }
            public List<string>? tags { get; set; } = new List<string>();
            public string created_at { get; set; }
            public int created_by_id { get; set; }
            public string modified_at { get; set; }
            public int modified_by_id { get; set; }
            public List<string>? permissions { get; set; } = new List<string>();
            public bool is_starred { get; set; }  //false
            public bool is_subscribed { get; set; } //false
            public bool is_trashed { get; set; } //false
        }

        public class InputListMembers 
        {
            //Path parameters
            public string app { get; set; } //uid

            //Query parameters
            public bool suspended { get; set; } //Indicates whether to list suspended members or not, null returns all members
            public string? q { get; set; } //A query used to find matching items
            public string? tag { get; set; } //List items with the specified tag
            public bool trashed { get; set; } = false;//Indicates whether trashed items should be listed (default is false). Specify null to return both trashed and non-trashed items.
            public string? order_by { get; set; } //Specifies the sort order and direction for the listing, e.g. "name" or "name+desc"
            public int? top { get; set; } //Maximum number of items to return in the listing. Should be a value between 1 and 100. Default is 25           
            public int? skip { get; set; } //The number of items to skip. Used together with top to return a specific range of items (for pagination)            
            public bool count_only { get; set; } //true to only return the number of matching items; when this is specified the response will only contain the count property.
        }

        public class OutputListMembers
        {
            public List<MembersData> data { get; set; } = new List<MembersData>();
            public int start { get; set; }
            public int end { get; set; }
            public int count { get; set; }

            //{
            //  "data": [
            //    {
            //      "id": "integer",
            //      "uid": "string",
            //      "access": "string",
            //      "display_name": "string",
            //      "avatar_url": "string",
            //      "delivered_at": "string",
            //      "marked_at": "string",
            //      "marked_id": "integer",
            //      "presence": "string",
            //      "is_bot": "boolean"
            //    }
            //  ],
            //  "start": "integer",
            //  "end": "integer",
            //  "count": "integer"
            //}
        }

        public class Members
        {
            public List<Object> data { get; set; } = new List<object>();
            public int start { get; set; }
            public int end { get; set; }
            public int count { get; set; }

            //"data": [
            //    "object"
            //],
            //"start": "integer",
            //"end": "integer",
            //"count": "integer"
        }

        public class MembersData
        {
            public int id { get; set; }
            public string uid { get; set; }
            public string access { get; set; }
            public string display_name { get; set; }
            public string avatar_url { get; set; }
            public string delivered_at { get; set; }
            public string marked_at { get; set; }
            public int marked_id { get; set; }
            public string? presence { get; set; }
            public bool is_bot { get; set; }  //false                
        }


    }

    public class By
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
        public List<string>? tags { get; set; } = new List<string>();
        public string? presence { get; set; }
        public string created_at { get; set; }
        public string modified_at { get; set; }
        public bool is_bot { get; set; }  //false
        public bool is_suspended { get; set; } //false
        public bool is_trashed { get; set; } //false

        //  "created_by": {
        //    "id": "integer",
        //    "uid": "string",
        //    "display_name": "string",
        //    "email": "string",
        //    "given_name": "string",
        //    "middle_name": "string",
        //    "name": "string",
        //    "family_name": "string",
        //    "nickname": "string",
        //    "phone_number": "string",
        //    "comment": "string",
        //    "directory_id": "integer",
        //    "picture_id": "integer",
        //    "avatar_url": "string",
        //    "metadata": "object",
        //    "tags": [
        //      "string"
        //    ],
        //    "presence": "string",
        //    "created_at": "string",
        //    "modified_at": "string",
        //    "is_bot": "boolean",
        //    "is_suspended": "boolean",
        //    "is_trashed": "boolean"
        //  },
    }    
}

    

