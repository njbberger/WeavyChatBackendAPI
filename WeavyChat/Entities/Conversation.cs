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
        }

        //curl https://{WEAVY-SERVER}/api/conversations
        //-H "Authorization: Bearer {ACCESS-TOKEN}"
        //--json "{ 'members': [ 'bugs-bunny' ] }"

        //curl https://{WEAVY-SERVER}/api/conversations
        //-H "Authorization: Bearer {ACCESS-TOKEN}"
        //--json "{ 'members': ['bugs-bunny', 'daffy-duck', 14], 'name': 'Acme'  }"

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

    }
}
