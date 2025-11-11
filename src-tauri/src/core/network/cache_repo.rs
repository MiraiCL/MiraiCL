pub trait ICacheRepository {
    fn get_etag_by_url(self,url:String);
}