#import "MyIOSInterface.h"
@implementation MyIOSInterface

- (void)showActionSheet
{
    NSLog(@" --- showActionSheet !!");
    
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction *albumAction = [UIAlertAction actionWithTitle:@"相册" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click album action!");
        [self showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:YES];
    }];
    
    UIAlertAction *cameraAction = [UIAlertAction actionWithTitle:@"相机" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click camera action!");
        [self showPicker:UIImagePickerControllerSourceTypeCamera allowsEditing:YES];
    }];
    
    UIAlertAction *album_cameraAction = [UIAlertAction actionWithTitle:@"相册&相机" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click album&camera action!");
        //[self showPicker:UIImagePickerControllerSourceTypeCamera];
        [self showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:YES];
    }];
    
    UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:@"取消" style:UIAlertActionStyleDefault handler:^(UIAlertAction * _Nonnull action) {
        NSLog(@"click cancel action!");
    }];
    
    
    [alertController addAction:albumAction];
    [alertController addAction:cameraAction];
    [alertController addAction:album_cameraAction];
    [alertController addAction:cancelAction];
    
    UIViewController *vc = UnityGetGLViewController();
    [vc presentViewController:alertController animated:YES completion:^{
        NSLog(@"showActionSheet -- completion");
    }];
}

- (void)showPicker:
(UIImagePickerControllerSourceType)type
     allowsEditing:(BOOL)flag
{
    NSLog(@" --- showPicker!!");
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.delegate = self;
    picker.sourceType = type;
    picker.allowsEditing = flag;
    
    [self presentViewController:picker animated:YES completion:nil];
}

// 打开相册后选择照片时的响应方法
- (void)imagePickerController:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
    NSLog(@" --- imagePickerController didFinishPickingMediaWithInfo!!");
    // Grab the image and write it to disk
    UIImage *image;
    UIImage *image2;
    image = [info objectForKey:UIImagePickerControllerEditedImage];
    UIGraphicsBeginImageContext(CGSizeMake(256,256));
    [image drawInRect:CGRectMake(0, 0, 256, 256)];
    image2 = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    // 得到了image，然后用你的函数传回u3d
    NSData *imgData;
    if(UIImagePNGRepresentation(image2) == nil)
    {
        NSLog(@" --- actionSheet slse!! 11 ");
        imgData= UIImageJPEGRepresentation(image, .6);
    }
    else
    {
        NSLog(@" --- actionSheet slse!! 22 ");
        imgData= UIImagePNGRepresentation(image2);
    }
    
    NSString *_encodeImageStr = [imgData base64Encoding];
    UnitySendMessage( "IOSAlbumCamera", "PickImageCallBack_Base64", _encodeImageStr.UTF8String);
    
    // 关闭相册
    [picker dismissViewControllerAnimated:YES completion:nil];
}

// 打开相册后点击“取消”的响应
- (void)imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
    NSLog(@" --- imagePickerControllerDidCancel !!");
    [self dismissViewControllerAnimated:YES completion:nil];
}

+(void) saveImageToPhotosAlbum:(NSString*) readAdr
{
    NSLog(@"readAdr: ");
    NSLog(readAdr);
    UIImage* image = [UIImage imageWithContentsOfFile:readAdr];
    UIImageWriteToSavedPhotosAlbum(image,
                                   self,
                                   @selector(image:didFinishSavingWithError:contextInfo:),
                                   NULL);
}

+(void) image:(UIImage*)image didFinishSavingWithError:(NSError*)error contextInfo:(void*)contextInfo
{
    NSString* result;
    if(error)
    {
        result = @"图片保存到相册失败!";
    }
    else
    {
        result = @"图片保存到相册成功!";
    }
    UnitySendMessage( "IOSAlbumCamera", "SaveImageToPhotosAlbumCallBack", result.UTF8String);
}
@end

//------------- called by unity -----begin-----------------
#if defined (__cplusplus)
extern "C" {
#endif
    
    void _Init()
    {
        NSLog(@" -unity call-- _showActionSheet !!");
        //        MyIOSInterface * app = [[MyIOSInterface alloc] init];
        //        UIViewController *vc = UnityGetGLViewController();
        //        [vc.view addSubview: app.view];
        //
        //        [app showActionSheet];
    }
    
    void _OpenPhotoLibrary_allowsEditing()
    {
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeSavedPhotosAlbum])
        {
            MyIOSInterface * app = [[MyIOSInterface alloc] init];
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: app.view];
            
            [app showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:YES];
        }
        else
        {
            if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary])
            {
                MyIOSInterface * app = [[MyIOSInterface alloc] init];
                UIViewController *vc = UnityGetGLViewController();
                [vc.view addSubview: app.view];
                
                [app showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:NO];
            }
            else
            {
                NSLog(@" -unity call-- PickImageCallBack_Base64 !!");
            }
        }
    }
    
    void _OpenPhotoLibrary()
    {
        if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeSavedPhotosAlbum])
        {
            MyIOSInterface * app = [[MyIOSInterface alloc] init];
            UIViewController *vc = UnityGetGLViewController();
            [vc.view addSubview: app.view];
            
            [app showPicker:UIImagePickerControllerSourceTypeSavedPhotosAlbum allowsEditing:NO];
        }
        else
        {
            if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary])
            {
                MyIOSInterface * app = [[MyIOSInterface alloc] init];
                UIViewController *vc = UnityGetGLViewController();
                [vc.view addSubview: app.view];
                
                [app showPicker:UIImagePickerControllerSourceTypePhotoLibrary allowsEditing:NO];
            }
            else
            {
                NSLog(@" -unity call-- PickImageCallBack_Base64 !!");
            }
        }
    }
    
    
    
    //保存图片到相册
    void _SaveImageToPhotosAlbum(char *readAddr)
    {
        NSString* temp = [NSString stringWithUTF8String:readAddr];
        [MyIOSInterface saveImageToPhotosAlbum:temp];
    }
    
    void  _SaveVideoToPhotosAlbum(char *path){
        NSLog(@"存入视频地址%s-------------",path);
        if (path == NULL) {
            return;
        }
        NSString *videoPath = [NSString stringWithUTF8String:path];
        ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
        [library writeVideoAtPathToSavedPhotosAlbum:[NSURL fileURLWithPath:videoPath]
                                    completionBlock:^(NSURL *assetURL, NSError *error) {
                                        if (error) {
                                            NSLog(@"Save video fail:%@",error);
                                        } else {
                                            NSLog(@"Save video succeed.");
                                        }
                                    }];
    }
    
    void _SharePictures(char *path,Boolean IsCircle)
    {
        //        NSString *strReadAddr = [NSString stringWithUTF8String:path];
        //        WXMediaMessage *message = [WXMediaMessage message];
        //        [message setThumbImage:[UIImage imageWithContentsOfFile:strReadAddr]];
        //        WXImageObject *imaageObject = [WXImageObject object];
        //        imaageObject.imageData = [NSData dataWithContentsOfFile:strReadAddr];
        //        message.mediaObject =imaageObject;
        //
        //        SendMessageToWXReq* req = [[SendMessageToWXReq alloc]init];
        //        req.bText = NO;
        //        req.message = message;
        //        req.scene = WXSceneTimeline;
        //        [WXApi sendReq:req];
    }
    
    void _ShareVideo(char *path,Boolean IsCircle)
    {
        //        NSString *strReadAddr = [NSString stringWithUTF8String:path];
        //        WXMediaMessage *message = [WXMediaMessage message];
        //        message.title = @"iket";
        //        message.description = @"iket";
        //
        //        WXVideoObject *videoObject = [WXVideoObject object];
        //        videoObject.videoUrl = strReadAddr;
        //
        //        SendMessageToWXReq* req = [[SendMessageToWXReq alloc]init];
        //        req.bText = NO;
        //        req.message = message;
        //        req.scene  = WXSceneSession;//分享到好友回话
        //
        //        [WXApi sendReq:req];
    }
    
#if defined (__cplusplus)
}
#endif

