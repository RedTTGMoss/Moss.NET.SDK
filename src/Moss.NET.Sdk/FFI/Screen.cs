/*
Screen & ChildScreen: - These are considered UI rendering objects and will have respective context handling
- key: str - An identifier for the screen
- screen_pre_loop: str - An extension function for the before loop call
- screen_loop: str - An extension function for the loop call
- screen_post_loop: str - An extension function for the after loop call
- event_hook: str - An extension function which will receive API events, must accept a single argument name: String
*/
public record Screen(string key, string screen_pre_loop, string screen_loop, string screen_post_loop, string event_hook);