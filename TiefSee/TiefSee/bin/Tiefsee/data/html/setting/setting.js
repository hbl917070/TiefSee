(function () {


    function getId(id) {
        return document.getElementById(id);
    }

    /**
     * html載入後
     */
    document.addEventListener('DOMContentLoaded', function () {


        //switch button
        var ar_switch = document.querySelectorAll('switch');
        for (let i = 0; i < ar_switch.length; i++) {
            var item = ar_switch[i];
            item.addEventListener("click", function () {
                if (this.getAttribute("type") == "off") {
                    this.setAttribute("type", "on");
                } else {
                    this.setAttribute("type", "off");
                }
            });

        }



        var tabs = new Tabs({
            cssName_but: 'tabs_but_on',
            cssName_tab: 'tabs_tab_on'
        });

        tabs.addItem('d_L_item_1', "tabs_1", function () { });
        tabs.addItem('d_L_item_2', "tabs_2", function () { });
        tabs.addItem('d_L_item_3', "tabs_3", function () { });
        tabs.addItem('d_L_item_4', "tabs_4", function () { });
        tabs.addItem('d_L_item_5', "tabs_5", function () { });
        tabs.addItem('d_L_item_6', "tabs_6", function () { });

        tabs.setTab(getId('tabs_1'))


        $('.colorbox').each(function () {
            //
            // Dear reader, it's actually very easy to initialize MiniColors. For example:
            //
            //  $(selector).minicolors();
            //
            // The way I've done it below is just for the demo, so don't get confused
            // by it. Also, data- attributes aren't supported at this time...they're
            // only used for this demo.
            //

            $(this).minicolors.defaults = $.extend($.minicolors.defaults, {
                animationSpeed: 5
            });


            $(this).minicolors({
                control: $(this).attr('data-control') || 'hue',
                defaultValue: $(this).attr('data-defaultValue') || '',
                format: $(this).attr('data-format') || 'hex',
                keywords: $(this).attr('data-keywords') || '',
                inline: $(this).attr('data-inline') === 'true',
                letterCase: $(this).attr('data-letterCase') || 'lowercase',
                opacity: $(this).attr('data-opacity'),
                position: $(this).attr('data-position') || 'bottom',
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                change: function (value, opacity) {
                    /*if( !value ) return;
                    if( opacity ) value += ', ' + opacity;
                    if( typeof console === 'object' ) {
                      console.log(value);
                    }*/
                },
                theme: 'bootstrap'

            });

        });


    });




    /**
     * 
     */
    var Tabs = function (option) {

        var cssName_but = option['cssName_but'];
        var cssName_tab = option['cssName_tab'];
        var ar_item = [];

        function getId(id) {
            return document.getElementById(id);
        }





        /**
         * 
         * @param {*} idBut 
         * @param {*} idTab 
         * @param {*} onSelect 
         */
        function addItem(idBut, idTab, onSelect) {

            let item = {
                idBut: getId(idBut),
                idTab: getId(idTab),
                onSelect: onSelect
            };
            ar_item.push(item);


            getId(idBut).onclick = function () {
                setTab(getId(idBut))
            }

        }


        /**
         * 
         * @param {*} obj 
         */
        function setTab(obj) {
            //document.body.setAttribute

            for (let i = 0; i < ar_item.length; i++) {
                let item = ar_item[i];

                if (obj === item['idBut'] || obj === item['idTab']) {

                    item['idBut'].setAttribute(cssName_but, 'on');
                    item['idTab'].setAttribute(cssName_tab, 'on');

                } else {
                    item['idBut'].setAttribute(cssName_but, 'off');
                    item['idTab'].setAttribute(cssName_tab, 'off');
                }
            }
        }


        return {
            addItem: addItem,
            setTab: setTab,

        };

    };





})();
