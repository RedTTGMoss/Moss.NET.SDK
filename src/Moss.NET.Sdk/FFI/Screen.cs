
/*
Screen & ChildScreen: - These are considered UI rendering objects and will have respective context handling
- key: str - An identifier for the screen
- screen_pre_loop: str - An extension function for the before loop call (optional)
- screen_loop: str - An extension function for the loop call
- screen_post_loop: str - An extension function for the after loop call (optional)
- event_hook: str - An extension function which will receive API events, must accept a single argument name: String (otpional)
*/
namespace Moss.NET.Sdk.FFI;

internal record Screen(string key, string screen_loop, string? screen_pre_loop = null, string? screen_post_loop = null, string? event_hook = null);