import Vue from "vue";
import iView from "view-design";
import Ajax from "./lib/ajax";
import Util from "./lib/util";
import SignalRAspNetCoreHelper from "./lib/SignalRAspNetCoreHelper";
import store from "./store/index";
import { router } from "./router/index";
import { appRouters, otherRouters } from "./router/router";
import App from "./app.vue";

import "famfamfam-flags/dist/sprite/famfamfam-flags.css";
import "./theme.less";

Vue.use(iView);

Vue.config.productionTip = false;

if (!abp.utils.getCookieValue("Abp.Localization.CultureName")) {
    let language = navigator.language;
    abp.utils.setCookieValue(
        "Abp.Localization.CultureName",
        language,
        new Date(new Date().getTime() + 5 * 365 * 86400000),
        abp.appPath
    );
}

Ajax.get("/AbpUserConfiguration/GetAll").then((data) => {
    Util.abp = Util.extend(true, Util.abp, data.data.result);
    new Vue({
        router: router,
        store: store,
        data: {
            currentPageName: "",
        },
        async mounted() {
            this.currentPageName = this.$route.name as string;

            await this.$store.dispatch({
                type: "session/init",
            });

            if (
                !!this.$store.state.session.user &&
                this.$store.state.session.application.features["SignalR"]
            ) {
                if (
                    this.$store.state.session.application.features[
                        "SignalR.AspNetCore"
                    ]
                ) {
                    SignalRAspNetCoreHelper.initSignalR();
                }
            }
            this.$store.commit("app/initCachepage");
            this.$store.commit("app/updateMenulist");
        },
        created() {
            let tagsList: Array<any> = [];
            appRouters.map((item) => {
                if (item.children.length <= 1) {
                    tagsList.push(item.children[0]);
                } else {
                    tagsList.push(...item.children);
                }
            });
            this.$store.commit("app/setTagsList", tagsList);
        },
        render: (h) => h(App),
    }).$mount("#app");
});
