
@implementation DialogPlugin_IOS : NSObject

static DialogPlugin_IOS *sharedInstance = nil;
NSInteger callbackCodes[4];
NSString* callbackGameObject;

+ (id)sharedInstance {
    @synchronized(self) {
        if(sharedInstance == nil)
            sharedInstance = [[super allocWithZone:NULL] init];
    }
    return sharedInstance;
}

- (void) ShowAlertMessage:(NSString*)callbackGO title:(NSString*)title message:(NSString*)message neutralButton:(NSString*)neutralButton negativeButton:(NSString*)negativeButton positiveButton:(NSString*)positiveButton cancelButton:(NSString*)cancelButton
{
    NSInteger index = 0;
    NSString* dismiss = nil;
    callbackGameObject = callbackGO;
    if (cancelButton.length > 0) {
        dismiss = cancelButton;
        callbackCodes[index] = 0;
        index++;
    }
    
    UIAlertView *baseAlert = [[UIAlertView alloc] initWithTitle: title
                                                        message: message
                                                       delegate: self
                                              cancelButtonTitle: dismiss
                                              otherButtonTitles: nil];
    
    //NSLog(@"XCODE stats callbackGO: %@, callbackGameObject: %@, title: %@, neutralButton %@, negativeButton %@, positiveButton %@, cancelButton %@", callbackGO, callbackGameObject, title, neutralButton, negativeButton, positiveButton, cancelButton);
    
    if (neutralButton.length > 0) {
        [baseAlert addButtonWithTitle:neutralButton];
        callbackCodes[index] = 1;
        index++;
    }
    if (negativeButton.length > 0) {
        [baseAlert addButtonWithTitle:negativeButton];
        callbackCodes[index] = 2;
        index++;
    }
    if (positiveButton.length > 0) {
        [baseAlert addButtonWithTitle:positiveButton];
        callbackCodes[index] = 3;
        index++;
    }
    [baseAlert show];
}

-(void) alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSString *tmp = [NSString stringWithFormat:@"%ld", (long)callbackCodes[buttonIndex]];
    const char *str = [tmp UTF8String];
    const char *callbackGO = [callbackGameObject UTF8String];
    NSLog(@"alertView result: %ld", (long)buttonIndex);
    
    UnitySendMessage(callbackGO, "CallbackFromOutside", str);
}

// Converts C style string to NSString
- (NSString*) CreateNSString:(const char*) string
{
    if (string && strlen(string) > 0)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

@end



// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
    extern void UnitySendMessage(const char *, const char *, const char *);
    
    void _ShowAlertMessage(const char* callbackGO, const char* title, const char* message, const char* neutralButton, const char* negativeButton, const char* positiveButton, const char* cancelButton)
    {
        DialogPlugin_IOS* bc = [DialogPlugin_IOS sharedInstance];
        NSString* callbackGOString = [bc CreateNSString:callbackGO];
        NSString* titleString = [bc CreateNSString:title];
        NSString* messageString = [bc CreateNSString:message];
        NSString* neutrString = [bc CreateNSString:neutralButton];
        NSString* negString = [bc CreateNSString:negativeButton];
        NSString* posString = [bc CreateNSString:positiveButton];
        NSString* cancelString = [bc CreateNSString:cancelButton];
        
        //NSLog(@"extern C, callbackGO: %@", callbackGOString);
        
        [bc ShowAlertMessage:callbackGOString title:titleString message:messageString neutralButton:neutrString negativeButton:negString positiveButton:posString cancelButton:cancelString];
    }
}


