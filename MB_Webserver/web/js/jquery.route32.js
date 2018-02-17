/**
 * Route32  --## Simple Anchor Location Router ##--
 * executes callback on location hash change that matches declared routes.
 * Intenteded to be use as a piece on JavaScript MVC Apps
 *
 * Copyright (C) 2010 by Rolando Garro
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * @author Rolando Garro <rgarro@gmail.com>
 * @requires jQuery
 */
//BEGIN...
//Requires jQuery
if(typeof jQuery != "undefined"){
    /* begin Route32 */
    function Route32(options){
        //Settings
        var settings = $.extend({
            //true executes routes from location hash changes, false from clicks
            'automatic':true,
            //selector from which we listen clicks when automatic false allow re-execution of same route
            'selector':'.nav',
            //manual drive is click based so needs an intentional delay to proper update latest hash
            'manualShiftChangeTime':100
        },options);
        //array of hashes containing hashsregexpstr,callbackfunc pairs
        var routes = [];

        var activeHash = '';

        var lastHash = '';

        //has variables
        var hasVariables = false;

        //methods
        var methods = {
            //initial method
            'init':function(){

            },
            //verifies is string is a valid location hash
            'isValidHash':function(hashStr){
                var testHashRegExp = new RegExp('^\#/([0-9a-zA-Z])');
                return testHashRegExp.test(hashStr);
            },
            'isValidCallbackfunc':function(callbackfunc){
                if(typeof callbackfunc == "function"){
                    return true;
                }else{
                    return false;
                }
            },
            'getHashValue':function(evt){
                //return window.location.hash;
                if(typeof evt.newURL != "undefined"){
                    return "#" + evt.newURL.split("#")[1];
                }else{
                    return methods.activeHashFromLocation();
                }

            },
            'activeHashFromLocation':function(){
                return "#" + window.location.hash.split("#")[1];
            },
            'hasVariables':function(){
                if(window.location.hash.split("#").length >2){
                    hasVariables = true;
                    return true;
                }else{
                    hasVariables = false;
                    return false
                }
            },
            //verifies if variable match #?name=vale&name=value format
            'isValidVariableString':function(varstr){
                return /^\?([0-9a-zA-Z]+=[0-9a-zA-Z])/.test(varstr);
            },
            'getVariables':function(){
                var varstr = '';
                varstr = window.location.hash.split("#")[2];
                var retObj = new Object();
                if(methods.isValidVariableString(varstr)){
                    var tmpstr = varstr.slice(1);
                    var p = tmpstr.split("&");
                    for(var i=0;i<p.length;i++){
                        var h = p[i].split("=");
                        var cmdstr = "retObj."+h[0]+" = '"+h[1]+"';";
                        eval(cmdstr);
                    }
                    return retObj;
                }else{
                    alert("Correct variable format is #?name=value sepaired by &.");
                }
            },
            'executeCurrent':function(){
                $.each(routes,function(index,value){
                    if(value.hash == activeHash){
                        if(methods.hasVariables()){
                            vars = methods.getVariables();
                            value.callback(vars);
                        }else{
                            value.callback();
                        }
                    }
                });
            },
            'updateHashExecute':function(){
                window.onhashchange = function(evt){
                    activeHash = methods.getHashValue(evt);
                    methods.executeCurrent();
                };
            },
            'manualDrive':function(){
                $(settings.selector).live('click',function(){
                    setTimeout(function(){
                        activeHash = methods.activeHashFromLocation();
                        methods.executeCurrent();
                    },settings.manualShiftChangeTime);
                });
            }
        };
        //variables can be passed after a second # singn on the hash as a query string
        this.getVariables = function(){

        }
        //adds routes
        this.add = function(hashRegexpStr,callbackfunc){
            if(methods.isValidHash(hashRegexpStr) && methods.isValidCallbackfunc(callbackfunc)){
                routes.push({hash:hashRegexpStr,callback:callbackfunc});
            }else{
                alert('route should be a valid hash string #/example/, callback function pair.');
            }
        };

        //starts driving
        this.drive = function(){
            if(routes.length > 0){
                if(settings.automatic){
                    //start listening location changes
                    methods.updateHashExecute();
                }else{
                    //listen selector click
                    methods.manualDrive();
                }
                activeHash = methods.activeHashFromLocation();
                if(activeHash.length > 1){//initial verification
                    methods.executeCurrent();
                }
            }else{
                alert('use add method to add routes');
            }
        };
        //executes actual route arbitrarily
        this.again = function(){
            methods.executeCurrent();
        };

        methods.init();

        return this;
    };
    /* end Route32 */
}else{
    alert("jQuery is required to ride Route32.");
}
//END..
