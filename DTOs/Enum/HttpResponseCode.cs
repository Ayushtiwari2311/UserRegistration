namespace DataTransferObjects.Enum
{
    public enum HttpResponseCode
    {
        #region Success
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        #endregion Success

        #region Client Error Response
        BadRequest = 400,
        Unauthorized = 401,
        #endregion Client Error Response

        #region Server error responses
        InternalServerError = 500,
        #endregion Server error responses

    }
}
