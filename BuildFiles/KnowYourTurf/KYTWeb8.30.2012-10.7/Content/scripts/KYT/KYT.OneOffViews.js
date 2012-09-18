/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 7/2/12
 * Time: 8:30 AM
 * To change this template use File | Settings | File Templates.
 */

KYT.Views.ResetPasswordView = KYT.Views.View.extend({
    initialize: function(){
        KYT.mixin(this, "baseFormView");
    },
    render: function(){
        KYT.notificationService = new cc.MessageNotficationService();
        this.bindModelAndElements();
        if(this.setBindings){this.setBindings();}
        $("input[name='Password']").focus();
        this.viewLoaded();
        KYT.vent.trigger("form:"+this.id+":pageLoaded",this.options);
        return this;
    }
});

KYT.Views.LoginView = KYT.Views.View.extend({
    registerEvents: function(){ this.events ={
        "click #forgotPasswordLink": "forgotPasswordClick",
        "click #registrationLink": "registrationLinkClick",
        "click .submit": "submitClick"
    }},
    initialize: function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "modelAndElementsMixin");
        this.registerEvents();
    },
    render: function () {
        this.bindModelAndElements();
        if (this.setBindings) { this.setBindings(); }
        $("input[name='UserName']").focus();
        this.viewLoaded();
        KYT.vent.trigger("form:" + this.id + ":pageLoaded", this.options);

        return this;
    },

    forgotPasswordClick: function (e) {
        e.preventDefault();
        var popupOptions = {
            url: this.options.forgotPasswordUrl,
            popupTitle: this.options.popupTitle
        };
        var forgottenPasswordView = new KYT.Views.AjaxPopupFormModule(popupOptions);
        forgottenPasswordView.render();

    },

    registrationLinkClick: function (e) {
        window.location.href = "/Registration/#newregistration";
    },

    submitClick: function (e) {
        var isValid = CC.ValidationRunner.runViewModel(this.elementsViewmodel);
        if(!isValid){return;}
        var data = JSON.stringify(ko.mapping.toJS(this.model));
        var promise = KYT.repository.ajaxPostModel(data.SaveUrl,data);
        promise.done(this.success);
    },
    success:function(result){
        if(result.Success){
            window.location.href = result.RedirectUrl;
        }else{
            CC.notification.handleResult(result);
        }

    }

});