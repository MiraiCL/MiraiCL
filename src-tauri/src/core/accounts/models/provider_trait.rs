pub trait AuthenticationProvider{
    async fn authenticate(self);
    async fn refresh(self);
    async fn validate(self);
    async fn invalidate(self);
}