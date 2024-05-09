using static WeavyChat.Entities.WeavyApp;

namespace WeavyChat.Entities
{
    public class Conversation
    {
        public class InputCreateConversation 
        {
            public List<string> members { get; set; } = new List<string>(); //User identifiers (id or uid) of conversation members
            public string? name { get; set; } = null; //Room name
            public string? type { get; set; } = null;//Type of conversation to create (private_chat, chat_room or bot_chat). Automatically inferred when not specified

            //curl https://{WEAVY-SERVER}/api/conversations
            //-H "Authorization: Bearer {ACCESS-TOKEN}"
            //--json "{ 'members': [ 'bugs-bunny' ] }"

            //curl https://{WEAVY-SERVER}/api/conversations
            //-H "Authorization: Bearer {ACCESS-TOKEN}"
            //--json "{ 'members': ['bugs-bunny', 'daffy-duck', 14], 'name': 'Acme'  }"
        }

        public class OutputCreateConversation
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
            public int? last_message_id { get; set; }
            public LastMessage last_message { get; set; } = new LastMessage();
            public bool is_starred { get; set; }
            public bool is_subscribed { get; set; }
            public bool is_trashed { get; set; }
            public bool is_pinned { get; set; }
            public bool is_unread { get; set; }

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
            //  "last_message_id": "integer",
            //  "last_message": {
            //    "id": "integer",
            //    "app_id": "integer",
            //    "text": "string",
            //    "html": "string",
            //    "plain": "string",
            //    "attachment_count": "integer",
            //    "attachments": [
            //      "object"
            //    ],
            //    "embed_id": "integer",
            //    "meeting_id": "integer",
            //    "option_count": "integer",
            //    "options": [
            //      "object"
            //    ],
            //    "reactions": [
            //      "object"
            //    ],
            //    "metadata": "object",
            //    "tags": [
            //      "string"
            //    ],
            //    "created_at": "string",
            //    "created_by_id": "integer",
            //    "modified_at": "string",
            //    "modified_by_id": "integer",
            //    "is_trashed": "boolean"
            //  },
            //  "is_starred": "boolean",
            //  "is_subscribed": "boolean",
            //  "is_trashed": "boolean",
            //  "is_pinned": "boolean",
            //  "is_unread": "boolean"
            //}

        }

        public class InputListConversations 
        {
            public string? member { get; set; } //User identifier (id or uid). Used to return conversations where specified user is member.
            public bool unread { get; set; } //true lists unread conversations, false list read conversations and null lists all conversations; default is null.
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

        public class OutputListConversations
        {
            public List<ConversationData> data { get; set; } = new List<ConversationData>();
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
            //      "last_message_id": "integer",
            //      "is_starred": "boolean",
            //      "is_subscribed": "boolean",
            //      "is_trashed": "boolean",
            //      "is_pinned": "boolean",
            //      "is_unread": "boolean"
            //    }
            //  ],
            //  "start": "integer",
            //  "end": "integer",
            //  "count": "integer"
            //}
        }

        public class LastMessage
        {
            public int id { get; set; }
            public int app_id { get; set; }
            public string text { get; set; }
            public string html { get; set; }
            public string plain { get; set; }
            public int attachment_count { get; set; }
            public List<object> attachments { get; set; } = new List<object>();
            public int embed_id { get; set; }
            public int meeting_id { get; set; }
            public int option_count { get; set; }
            public List<object> options { get; set; } = new List<object>();
            public List<object> reactions { get; set; } = new List<object>();
            public object metadata { get; set; }
            public List<string> tags { get; set; } = new List<string>();
            public string created_at { get; set; }
            public int created_by_id { get; set; }
            public string modified_at { get; set; }
            public int modified_by_id { get; set; }
            public bool is_trashed { get; set; }

            // "last_message": {
            //    "id": "integer",
            //    "app_id": "integer",
            //    "text": "string",
            //    "html": "string",
            //    "plain": "string",
            //    "attachment_count": "integer",
            //    "attachments": [
            //      "object"
            //    ],
            //    "embed_id": "integer",
            //    "meeting_id": "integer",
            //    "option_count": "integer",
            //    "options": [
            //      "object"
            //    ],
            //    "reactions": [
            //      "object"
            //    ],
            //    "metadata": "object",
            //    "tags": [
            //      "string"
            //    ],
            //    "created_at": "string",
            //    "created_by_id": "integer",
            //    "modified_at": "string",
            //    "modified_by_id": "integer",
            //    "is_trashed": "boolean"
            //  },
        }

        public class ConversationData
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
            public int last_message_id { get; set; }            
            public bool is_starred { get; set; }  
            public bool is_subscribed { get; set; } 
            public bool is_trashed { get; set; } 
            public bool is_pinned { get; set; } 
            public bool is_unread { get; set; } 
        }
    }
}
