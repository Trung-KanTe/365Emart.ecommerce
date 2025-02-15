namespace Commerce.Query.Contract.Constants
{
    public static class Const
    {

        #region Weblocalization config

        public const string GET_WEBLOCAL_BY_WARD_IDS_REQUEST_ENTITY_NAME = "topic.get_weblocal_by_wardids_request";
        public const string GET_WEBLOCAL_BY_WARD_IDS_RESPONSE_ENTITY_NAME = "topic.get_weblocal_by_wardids_response";
        public const string GET_WEBLOCAL_BY_WARD_IDS_REQUEST_URN = "urn:type.get_weblocal_by_wardids_request";
        public const string GET_WEBLOCAL_BY_WARD_IDS_RESPONSE_URN = "urn:type.get_weblocal_by_wardids_response";

        #endregion

        #region rabbitMQ

        public const string BROKER_CONFIG = "MessageBroker";
        public const string BROKER_HOST = "Host";
        public const string BROKER_USERNAME = "Username";
        public const string BROKER_PASSWORD = "Password";

        #endregion
    }
}