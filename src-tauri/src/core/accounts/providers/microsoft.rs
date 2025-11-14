use crate::core::accounts::models::provider_trait::AuthenticationProvider;

pub struct MicrosoftProvider {
    access_token: String,
    refresh_token: String,
}
/*
impl AuthenticationProvider for MicrosoftProvider{
    fn new() -> MicrosoftProvider{
        MicrosoftProvider{
            access_token:"sss".to_string(),
            refresh_token:"sss".to_string()
        }
    }

    async fn authenticate(self){

    }

    async fn refresh(self){

    }

    async fn validate(self) {

    }

    async fn invalidate(self) {

    }
}

    */
