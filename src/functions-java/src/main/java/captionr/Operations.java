package captionr;

import java.util.*;
import java.util.stream.Collectors;

import com.microsoft.azure.functions.annotation.*;
import com.microsoft.azure.functions.signalr.SignalRConnectionInfo;
import com.microsoft.azure.functions.signalr.SignalRGroupAction;
import com.microsoft.azure.functions.signalr.SignalRMessage;
import com.microsoft.azure.functions.signalr.annotation.SignalRConnectionInfoInput;
import com.microsoft.azure.functions.signalr.annotation.SignalROutput;
import com.microsoft.azure.functions.*;

import captionr.common.*;


public class Operations {
    @FunctionName("negotiate")
    public HttpResponseMessage negotiate(
            @HttpTrigger(name = "req", methods = {HttpMethod.POST}, authLevel = AuthorizationLevel.ANONYMOUS, route = "{userId}/negotiate")
                HttpRequestMessage<Optional<String>> req,
            @SignalRConnectionInfoInput(name = "connectionInfo", hubName = "captions", userId = "{userId}")
                SignalRConnectionInfo connectionInfo) {
        
            return req
                .createResponseBuilder(HttpStatus.OK)
                .header("Content-type", "application/json")
                .body(connectionInfo)
                .build();
    }

    @FunctionName("languages")
    public HttpResponseMessage languages(
        @HttpTrigger(name = "req", methods = {HttpMethod.GET}, authLevel = AuthorizationLevel.ANONYMOUS)
            HttpRequestMessage<Optional<String>> req) {

        return req
            .createResponseBuilder(HttpStatus.OK)
            .header("Content-type", "application/json")
            .body(Constants.languages)
            .build();
    }

    @FunctionName("selectLanguage")
    public void selectLanguage(
        @HttpTrigger(name = "req", methods = {HttpMethod.POST}, authLevel = AuthorizationLevel.ANONYMOUS)
            HttpRequestMessage<LanguageSelection> req,
        @SignalROutput(name = "signalRActions", hubName = "captions")
            OutputBinding<List<SignalRGroupAction>> signalRActions) {

        LanguageSelection payload = req.getBody();
        
        signalRActions.setValue(
            Constants.languageCodes
                .stream()
                .map(lc -> new SignalRGroupAction(lc.equals(payload.languageCode) ? "add" : "remove", lc, payload.userId))
                .collect(Collectors.toList()));
    }

    @FunctionName("captions")
    public void captions(
        @HttpTrigger(name = "req", methods = {HttpMethod.POST}, authLevel = AuthorizationLevel.ANONYMOUS)
            HttpRequestMessage<CaptionsRequest> req,
        @SignalROutput(name = "signalRMessages", hubName = "captions")
            OutputBinding<List<SignalRMessage>> signalRMessages) {

        CaptionsRequest payload = req.getBody();
        List<LanguageCaption> captionsByLanguage = 
            payload.languages.entrySet().stream()
                .map(l -> new LanguageCaption(payload.offset, l.getKey(), l.getValue()))
                .collect(Collectors.toList());

        signalRMessages.setValue(captionsByLanguage.stream().map(c -> {
            SignalRMessage msg = new SignalRMessage();
            msg.target = "newCaption";
            msg.arguments.add(c);
            msg.groupName = c.language;
            return msg;
        }).collect(Collectors.toList()));
    }
}
